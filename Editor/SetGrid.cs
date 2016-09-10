using UnityEditor;
using UnityEngine;
using System.IO;

public class SetGrid : Editor
{
    public GameObject grid;

    public void LoadGrid()
    {
        if (!Directory.Exists(Constants.resourcePath))
            EditorUtility.DisplayDialog(Constants.errorString, "Failed to load the grid because the folder 'Resource' does not exists in the 'Assets'.", Constants.okString);
        else if (!File.Exists(Constants.resourcePath + "Grid.prefab"))
            EditorUtility.DisplayDialog(Constants.errorString, "Failed to load the grid because the prefab 'Grid' does not exists in the 'Resources' folder. " +
                                                               "Create a empty GameObject in the scene and put the 'Grid.cs' script on it. Click and drag the object to the 'Resources' folder and rename it to 'Grid'.", Constants.okString);
        else if (!grid)
            grid = Resources.Load<GameObject>("Grid");
    }
}
