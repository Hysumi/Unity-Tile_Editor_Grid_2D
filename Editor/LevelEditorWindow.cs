using UnityEngine;
using UnityEditor;
public class LevelEditorWindow : EditorWindow
{
    #region Layers and Tags Variables

    string[] backgroundLayerNames = new string[Constants.maxBackgroundLayerLenght], 
             layerNames = new string[Constants.maxLayerLenght],
             frontLayerNames = new string[Constants.maxFrontLayerLenght],
             tagNames = new string[Constants.maxTagLenght],
             layerTab = new string[] { "Back Layer", "Main Layer", "Front Layer" };
    public string ln = "", ln2 = "", tn = "", tn2 = "";
    public int currentTab, indexB = 0, indexF = 0, indexLayers = 0, indexT = 0, indexTag = 0;
    public LayersAndTags layersandtags;

    #endregion

    public Rect windowRect;

    #region Grid Variables

    public SetGrid setGrid;
    public GameObject grid;

    #endregion

    void OnEnable()
    {
        if (!layersandtags)
            layersandtags = ScriptableObject.CreateInstance<LayersAndTags>();
        layerNames = layersandtags.editLayers(layerNames, ln2, indexLayers);
        tagNames = layersandtags.getTags();

        if (!setGrid)
            setGrid = ScriptableObject.CreateInstance<SetGrid>();
        setGrid.LoadGrid();
        if (!grid)
            grid = Instantiate(setGrid.grid);
    }

    void OnHierarchyChange()
    {
        if (!GameObject.Find("Grid(Clone)") && Application.isEditor && !Application.isPlaying)
        {
            setGrid.LoadGrid();
            grid = Instantiate(setGrid.grid);
        }     
    }

    void OnDisable()
    {
        if (grid)
            DestroyImmediate(grid);
    }

    void OnGUI() //Aqui se implementa o editor
    {
        #region Preenche os Background e Front Layers

        for (int i = 0; i < backgroundLayerNames.Length; i++)
            backgroundLayerNames[i] = layerNames[i + 8];
        for (int i = 0; i < frontLayerNames.Length-1; i++)
            frontLayerNames[i] = layerNames[i + 21];
        
        #endregion

        windowRect = new Rect(this.position.x, this.position.y, this.position.width, this.position.height); //Pega um Rect do tamanho da janela
        
        #region Abas

        currentTab = GUILayout.Toolbar(currentTab, layerTab); //Cria a toolbar com as 3 abas
        switch (currentTab)
        {
            case 0:

                #region Selecionar Layer e Definir nome da Layer

                GUILayout.Box("", GUILayout.Width(windowRect.width - 5), GUILayout.Height(133)); //Box de Layers and Tags 
                GUI.Label(new Rect(5, 25, 200, 200), "Select Back Layer: ");
                indexB = EditorGUI.Popup(new Rect(180, 27, 100, 15), indexB, backgroundLayerNames);
                GUI.Label(new Rect(5, 45, 200, 200), "Define Back Layer Name: ");
                ln = layerNames[indexB + 8];
                ln2 = EditorGUI.TextField(new Rect(180, 47, 100, 15), ln);
                indexLayers = 8 + indexB;
                layerNames = layersandtags.editLayers(layerNames, ln2, indexLayers);
                
                #endregion

                break;
            case 1:
                GUILayout.Box("", GUILayout.Width(windowRect.width - 5), GUILayout.Height(113)); //Box de Layers and Tags 
                GUI.Label(new Rect(5, 25, 200, 200), "Main Layer Selected", EditorStyles.boldLabel);
                GUI.TextField(new Rect(180, 27, 100, 15), "Main Layer");
                break;
            case 2:

                #region Selecionar Layer e Definir nome da Layer

                GUILayout.Box("", GUILayout.Width(windowRect.width - 5), GUILayout.Height(133)); //Box de Layers and Tags 
                GUI.Label(new Rect(5, 25, 200, 200), "Select Front Layer: ");
                indexF = EditorGUI.Popup(new Rect(180, 27, 100, 15), indexF, frontLayerNames);
                GUI.Label(new Rect(5, 45, 200, 200), "Define Front Layer Name: ");
                ln = layerNames[indexF + 21];
                ln2 = EditorGUI.TextField(new Rect(180, 47, 100, 15), ln);
                
                indexLayers = 21 + indexF;
                layerNames = layersandtags.editLayers(layerNames, ln2, indexLayers);

                #endregion

                break;
        }

        #region Tags and Reset

        if(currentTab == 1) //Se for Main Layer
        {
            GUI.Label(new Rect(5, 45, 200, 200), "Select Tag: ");
            indexT = EditorGUI.Popup(new Rect(180, 47, 100, 15), indexT, tagNames);
            GUI.Label(new Rect(5, 65, 200, 200), "Add/Edit/Remove Tag: ");
            tn = tn2;
            tn2 = EditorGUI.TextField(new Rect(180, 67, 100, 15), tn);

            if (GUI.Button(new Rect(13, 87, 80, 20), "Add"))
                tagNames = layersandtags.addTags(tn2);
            if (GUI.Button(new Rect(103, 87, 80, 20), "Edit"))
                tagNames = layersandtags.editTag(tagNames, tn2, indexT);
            if (GUI.Button(new Rect(193, 87, 80, 20), "Remove"))
                tagNames = layersandtags.removeTag(tn2);

            if (GUI.Button(new Rect(13, 112, 260, 20), "Clean Tags and Layers"))
            {
                bool resposta = EditorUtility.DisplayDialog("WARNING!!", "Do you really want to delete all of your layers and tags?", "Just do it!", "OH GOD, NO!");
                if (resposta)
                    layersandtags.resetLayersAndTags();
            }

        }
        else
        {
            GUI.Label(new Rect(5, 65, 200, 200), "Select Tag: ");
            indexT = EditorGUI.Popup(new Rect(180, 67, 100, 15), indexT, tagNames);
            GUI.Label(new Rect(5, 85, 200, 200), "Add/Edit/Remove Tag: ");
            tn = tn2;
            tn2 = EditorGUI.TextField(new Rect(180, 87, 100, 15), tn);

            if (GUI.Button(new Rect(13, 107, 80, 20), "Add"))
                tagNames = layersandtags.addTags(tn2);
            if (GUI.Button(new Rect(103, 107, 80, 20), "Edit"))
                tagNames = layersandtags.editTag(tagNames, tn2, indexT);
            if (GUI.Button(new Rect(193, 107, 80, 20), "Remove"))
                tagNames = layersandtags.removeTag(tn2);

            if (GUI.Button(new Rect(13, 132, 260, 20), "Clean Tags and Layers"))
            {
                bool resposta = EditorUtility.DisplayDialog("WARNING!!", "Do you really want to delete all of your layers and tags?", "Just do it!", "OH GOD, NO!");
                if (resposta)
                    layersandtags.resetLayersAndTags();
            }
        }

        #endregion

        #region Grid Configuration
        
        
        
        #endregion

        #endregion

    }
}
