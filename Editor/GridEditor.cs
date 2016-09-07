using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))] //Ligado ao script Grid
public class GridEditor : Editor
{
    Grid grid;

    private int oldIndex = 0;

    void OnEnable() //Chama semrpe que o programa estiver rodando (Unity) 
    {
        grid = (Grid)target; //O target é dado pelo CustomEditor
    }

    public override void OnInspectorGUI() //Sobrescreve com o conteúdo do Script "Grid" no inspector da Unity, podendo criar o próprio layout
    {
        //base.OnInspectorGUI(); //Mostra o conteúdo base que já existia no código
        grid.width = CreateSlider("Width", grid.width);
        grid.height = CreateSlider("Height", grid.height);

        grid.color = EditorGUILayout.ColorField("Grid Color", grid.color);

        #region Cria uma Grid Window
        //Abre um Grid Color, Comentar depois
        if (GUILayout.Button("Open Grid Window")) //Se esse botão é apertado...
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow)); //Procura o EditorWindow
            window.init(); //Inicializa a janela
        }

        #endregion

        #region Tile Prefab

        EditorGUI.BeginChangeCheck(); //Testa se alguma coisa foi alterada (Async)
        //Escolhe-se o tipo do prefab a partir do ObjectField (nesse caso, o tipo é Transform)
        var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);

        if (EditorGUI.EndChangeCheck()) //Se algo foi alterado
        {
            grid.tilePrefab = newTilePrefab; //Coloca no Grid o newTilePrefab
            Undo.RecordObject(target, "Grid Changed"); //Pode dar Undo
        }

        #endregion

        #region Tile Map

        EditorGUI.BeginChangeCheck();

        var newTileSet = (TileSet)EditorGUILayout.ObjectField("TileSet", grid.tileSet, typeof(TileSet), false);
        if (EditorGUI.EndChangeCheck())
        {
            grid.tileSet = newTileSet;
            Undo.RecordObject(target, "Grid Changed");
        }

        if(grid.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            var names = new string[grid.tileSet.prefabs.Length]; //Numero de prefabs no tileSet
            var values = new int[names.Length];
            for (int i = 0; i < names.Length; i++) //Para juntar os nomes em relação aos prefabs do tileSet
            {
                names[i] = grid.tileSet.prefabs[i] != null ? grid.tileSet.prefabs[i].name : "";
                values[i] = i;
            }

            var index = EditorGUILayout.IntPopup("Select Tile", oldIndex, names, values);

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Grid Changed");
                if(oldIndex != index)
                {
                    oldIndex = index;
                    grid.tilePrefab = grid.tileSet.prefabs[index];
                }

                float width = grid.tilePrefab.GetComponent<Renderer>().bounds.size.x;
                float height = grid.tilePrefab.GetComponent<Renderer>().bounds.size.y;

                grid.width = width;
                grid.height = height;
            }
        }

        #endregion

    }

    private float CreateSlider(string labelName, float sliderPosition)
    {
        #region Criando um Slider para o Grid Width/Height

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid " + labelName); // Só um TextLabel
        sliderPosition = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
        GUILayout.EndHorizontal();

        #endregion

        return (sliderPosition);
    }

    #region Colocando/Apagando objetos no editor da cena 2D

    void OnSceneGUI() //É chamado quando se faz alguma ação no editor da cena
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePosition = ray.origin;

        if (e.isMouse && e.type == EventType.MouseDown && e.button == 0) //Se clicou com o botão esquerdo
        {
            GUIUtility.hotControl = controlId;
            e.Use();

            GameObject gameObject; //Cria o objeto que será posto na cena
            Transform prefab = grid.tilePrefab;

            if (prefab)
            {
                Undo.IncrementCurrentGroup();
                Vector3 aligned = new Vector3(Mathf.Floor(mousePosition.x / grid.width) * grid.width + grid.width/2.0f, 
                                              Mathf.Floor(mousePosition.y / grid.height) * grid.height + grid.height / 2.0f, 0.0f);

                if (GetTransformFromPosition(aligned) != null) return;//Se tiver algo no lugar do tile já, ele nem instancia

                gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                gameObject.transform.position = aligned;
                gameObject.transform.parent = grid.transform; //Vai ser filho objeto que tem o script do Grid
                Undo.RegisterCreatedObjectUndo(gameObject, " Create " + gameObject.name);
                
            }
        }
        
        if (e.isMouse && e.type == EventType.MouseDown && e.button == 1) //Clicando com o direito
        {
            GUIUtility.hotControl = controlId;
            e.Use();
            Vector3 aligned = new Vector3(Mathf.Floor(mousePosition.x / grid.width) * grid.width + grid.width / 2.0f,
                                          Mathf.Floor(mousePosition.y / grid.height) * grid.height + grid.height / 2.0f, 0.0f);
            Transform transform = GetTransformFromPosition(aligned);
            if(transform != null)
            {
                DestroyImmediate(transform.gameObject);
            }
        }
        if (e.isMouse && e.type == EventType.MouseUp)
        {
            GUIUtility.hotControl = 0; //O hotControl não está mais preso ao editor da cena
        }
    }

    #endregion

    Transform GetTransformFromPosition(Vector3 aligned)
    {
        int i = 0;
        while(i<grid.transform.childCount)
        {
            Transform transform = grid.transform.GetChild(i);
            if (transform.position == aligned)
            {
                return transform;
            }
            i++;
        }
        return null;
    }
}
