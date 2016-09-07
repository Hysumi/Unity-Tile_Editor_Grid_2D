using UnityEditor;
using UnityEngine;

public class LayersAndTags : Editor
{
    #region Load Variables

    public string[] oldLayerNames = new string[32];

    #endregion

    public string[] createLayers(string[] layerNames)
    {
        oldLayerNames[20] = "Main Layer";

        #region Verifica as layers

        bool hasMain = false, changeLayers = false;
        bool[] changedLayerName = new bool[32]; 
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        for (int a = 0; a <= 31; a++)
        {
            SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(a);
            layerNames[a] = layerVal.stringValue;
            if (layerNames[a] != oldLayerNames[a])
            {
                changedLayerName[a] = true;
                changeLayers = true;
            }
            else if (layerNames[a] == "Main Layer" && a == 20)
            {
                hasMain = true;
                changedLayerName[a] = false;
            }
            else if (layerNames[a] == "")
                changeLayers = true;
            else changedLayerName[a] = false;
        }
        if (!hasMain)
            changeLayers = true;
        

        #endregion

        #region Altera as Layers

        if (changeLayers)
        {
            for (int a = 8; a <= 31; a++)
            {
                SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(a);
                if (changedLayerName[a])
                {
                    oldLayerNames[a] = layerNames[a];
                    layerVal.stringValue = layerNames[a];
                }
                else if (layerNames[20] == "" && layerNames[20] != "Main Layer" && a == 20)
                {
                    if (!hasMain)
                    {
                        layerVal.stringValue = "Main Layer";
                        hasMain = true;
                    }
                }
                else if (layerNames[a] == "")
                    layerVal.stringValue = "Layer " + a;

            }
            changeLayers = false;
        }

        #endregion
        
        // Save changes.
        tagManager.ApplyModifiedProperties();
        return (oldLayerNames);

    }
}
