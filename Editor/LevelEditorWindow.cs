using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : EditorWindow
{
    string[] layerNames = new string[32];
    string[] backgroundLayerNames = new string[13];
    string[] frontLayerNames = new string[12];
    string[] layerTab = new string[] { "Background Layer", "Main Layer", "Front Layer" };
    public Rect windowRect;
    public int currentTab, index;
    public LayersAndTags layersandtags;

    void OnInspectorUpdate()
    {
        if (!layersandtags)
            layersandtags = ScriptableObject.CreateInstance<LayersAndTags>();
        layerNames = layersandtags.createLayers(layerNames);
    }

    void OnGUI() //Aqui se implementa o editor
    {
        for (int i = 0; i < backgroundLayerNames.Length; i++)
            backgroundLayerNames[i] = layerNames[i + 7];
        for (int i = 0; i < frontLayerNames.Length; i++)
            frontLayerNames[i] = layerNames[i + 21];

        windowRect = new Rect(this.position.x, this.position.y, this.position.width, this.position.height); //Pega um Rect do tamanho da janela

        #region Abas

        currentTab = GUILayout.Toolbar(currentTab, layerTab); //Cria a toolbar com as 3 abas
        Event e = Event.current;
        GUILayout.Box("", GUILayout.Width(windowRect.width - 5), GUILayout.Height(100)); //Setting Box
        switch (currentTab)
        {
            case 0:
                GUI.Label(new Rect(5, 25, 200, 200), "Select Background Layer: ");
                index = EditorGUI.Popup(new Rect(160, 27, 100, 20), index, backgroundLayerNames);
                break;
            case 1:
                GUI.Label(new Rect(5, 25, 200, 200), "Main Layer Selected");
                break;
            case 2:
                GUI.Label(new Rect(5, 25, 200, 200), "Select Front Layer: ");
                index = EditorGUI.Popup(new Rect(160, 27, 100, 20), index, frontLayerNames);
                break;
        }

        #endregion
    }
}
