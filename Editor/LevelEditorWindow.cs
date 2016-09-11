using UnityEngine;
using UnityEditor;
public class LevelEditorWindow : EditorWindow
{
    #region Layers and Tags Variables

    string[] backgroundLayerNames = new string[Constants.maxBackgroundLayerLenght], 
             layerNames = new string[Constants.maxLayerLenght],
             frontLayerNames = new string[Constants.maxFrontLayerLenght],
             tagNames = new string[Constants.maxTagLenght],
             layerTab = new string[] { "Back Layer", "Main Layer", "Front Layer"};
    public string ln = "", ln2 = "", tn = "", tn2 = "";
    public int currentTab, indexB = 0, indexF = 0, indexLayers = 0, indexT = 0, indexTag = 0;
    public Vector2 maxScroll;
    public LayersAndTags layersandtags;

    #endregion

    public Rect windowRect;
    Vector2 scrollPos;

    #region Grid Variables

    public SetGrid setGrid;
    public GameObject grid;
    public Grid g;
    public GridEditor gridEditor;

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

        if (!grid && !GameObject.Find("Grid(Clone)"))
        {
            grid = Instantiate(setGrid.grid);
            g = grid.GetComponent<Grid>();
        }
        else if (!grid)
        {
            grid = GameObject.Find("Grid(Clone)");
            g = grid.GetComponent<Grid>();
        }
    }
    
    void OnHierarchyChange()
    {
        if (!GameObject.Find("Grid(Clone)") && Application.isEditor && !Application.isPlaying)
        {
            setGrid.LoadGrid();
            grid = Instantiate(setGrid.grid);
            g = grid.GetComponent<Grid>();
        }
    }


    void OnGUI()
    {
        windowRect = new Rect(this.position.x, this.position.y, this.position.width, this.position.height); //Pega um Rect do tamanho da janela

        #region Abas

        currentTab = GUILayout.Toolbar(currentTab, layerTab); //Cria a toolbar com as 3 abas
        scrollPos = GUI.BeginScrollView(new Rect(0, 25, windowRect.width, windowRect.height-10), 
                    scrollPos, new Rect(0, 0, maxScroll.x, maxScroll.y),false, false);
       
        switch (currentTab)
        {
            case 0:
                LayersOptions("Back", ref indexB, backgroundLayerNames, 8);
                break;
            case 1:
                GUI.Box(new Rect(0, 0, windowRect.width, 119), "");
                GUI.Label(new Rect(5, 5, 200, 200), "Main Layer Selected", EditorStyles.boldLabel);
                GUI.TextField(new Rect(180, 7, 100, 15), "Main Layer");
                break;
            case 2:
                LayersOptions("Front", ref indexF, frontLayerNames, 21);
                break;
        }

        if (currentTab == 1) //Se for Main Layer
            TagsAndResetOptions(0);
        else
            TagsAndResetOptions(Constants.mainLayerHeightDifference);


        #endregion

        #region Grid Configuration

        if (currentTab == 1) //MainLayer
            GridOptions(0);
        else
            GridOptions(Constants.mainLayerHeightDifference);

        #endregion

        maxScroll = new Vector2(300, 257);

        GUI.EndScrollView();
    }

    #region Preenche os Background e Front Layers

    void FillLayers()
    {
        for (int i = 0; i < backgroundLayerNames.Length; i++)
            backgroundLayerNames[i] = layerNames[i + 8];
        for (int i = 0; i < frontLayerNames.Length; i++)
            frontLayerNames[i] = layerNames[i + 21];
    }

    #endregion

    #region Layers

    void LayersOptions(string layer, ref int index, string[] names, int pos)
    {
        FillLayers();

        GUI.Box(new Rect(0, 0, windowRect.width, 139), "");
        GUI.Label(new Rect(5, 5, 200, 200), "Select " + layer + " Layer: ");
        index = EditorGUI.Popup(new Rect(180, 7, 100, 15), index, names);
        GUI.Label(new Rect(5, 25, 200, 200), "Edit " + layer + " Layer Name: ");
        ln = layerNames[index + pos];
        ln2 = EditorGUI.TextField(new Rect(180, 27, 100, 15), ln);
        indexLayers = pos + index;
        layerNames = layersandtags.editLayers(layerNames, ln2, indexLayers);
    }
    
    #endregion

    #region Tags and Reset

    void TagsAndResetOptions(float height)
    {
        GUI.Label(new Rect(5, 25 + height, 200, 200), "Select Tag: ");
        indexT = EditorGUI.Popup(new Rect(180, 27 + height, 100, 15), indexT, tagNames);
        GUI.Label(new Rect(5, 45 + height, 200, 200), "Add/Edit/Remove Tag: ");
        tn = tn2;
        tn2 = EditorGUI.TextField(new Rect(180, 47 + height, 100, 15), tn);

        if (GUI.Button(new Rect(13, 67 + height, 80, 20), "Add"))
            tagNames = layersandtags.addTags(tn2);
        if (GUI.Button(new Rect(103, 67 + height, 80, 20), "Edit"))
            tagNames = layersandtags.editTag(tagNames, tn2, indexT);
        if (GUI.Button(new Rect(193, 67 + height, 80, 20), "Remove"))
            tagNames = layersandtags.removeTag(tn2);

        if (GUI.Button(new Rect(13, 92 + height, 260, 20), "Reset Tags and Layers"))
        {
            bool resposta = EditorUtility.DisplayDialog(Constants.warningString, "Do you really want to delete all of your layers and tags?", "Just do it!", "OH GOD, NO!");
            if (resposta)
                layersandtags.resetLayersAndTags();
        }
    }

    #endregion

    #region Grid

    void GridOptions(float height)
    {
        GUI.Box(new Rect(0, 122 + height, windowRect.width, 89), "");

        GUI.Label(new Rect(5, 127 + height, 200, 200), "Grid Options", EditorStyles.boldLabel);

        #region Sliders Width Height

        GUI.Label(new Rect(5, 147 + height, 200, 200), "Grid Width: ");
        g.width = EditorGUI.Slider(new Rect(80, 147 + height, 200, 17), g.width, 1f, 100f);
        GUI.Label(new Rect(5, 167 + height, 200, 200), "Grid Height: ");
        g.height = EditorGUI.Slider(new Rect(80, 167 + height, 200, 17), g.height, 1f, 100f);

        #endregion

        #region Grid Color

        GUI.Label(new Rect(5, 187 + height, 200, 200), "Grid Color: ");
        g.color = EditorGUI.ColorField(new Rect(180, 187 + height, 100, 17), g.color);

        #endregion

    }

    #endregion

}
