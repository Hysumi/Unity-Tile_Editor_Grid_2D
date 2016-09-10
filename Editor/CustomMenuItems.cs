using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomMenuItems : Editor {

    #region Criando um TileSet no menu Create da Unity

    [MenuItem("Assets/Create/TileSet")] //Cria um item no menu do tipo TileSet
    static void CreateTileSet()
    {
        var asset = CreateInstance<TileSet>(); //Cria uma instância de TileSet
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
            path = "Assets";
        else if (Path.GetExtension(path) != "")
            path = path.Replace(Path.GetFileName(path), "");
        else
            path += "/";

        var assetPathAdName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset"); //Gera um caminho único, evitando overrides
        AssetDatabase.CreateAsset(asset, assetPathAdName); //Cria o Asset com o nome e o caminho
        AssetDatabase.SaveAssets(); //Salva o Asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;  //Torna independente da cena, ou seja, não será deletado numa nova cena

    }

    #endregion

    #region Criando uma nova Window na barra de tarefas do menu "Window"

    [MenuItem("Window/Level Editor 2D")]
    static void Init()
    {
        //Cria uma nova Window caso não tenha uma aberta
        LevelEditorWindow window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));
        window.titleContent.text = "Level Editor";
        window.minSize = new Vector2(286f, 170f);
        window.Show();
    }

    #endregion
}
