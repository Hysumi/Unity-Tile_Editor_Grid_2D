using UnityEditor;
using UnityEngine;

public class LayersAndTags : Editor
{
    #region Load Variables

    public string[] oldLayerNames = new string[Constants.maxLayerLenght];
    public string[] oldTagNames = new string[Constants.maxTagLenght];
    string newlayerName;
            int oldindex;


    #endregion


    #region Edita as Layers

    public string[] editLayers(string[] layerNames, string ln, int index)
    {
        oldLayerNames[20] = "Main Layer";

        #region Verifica as layers
        bool hasMain = false, changeLayers = false, equalLayername = false;
        bool[] changedLayerName = new bool[Constants.maxLayerLenght];
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        for (int i = 0; i <= Constants.maxLayerLenght - 1; i++)
        {
            SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(i);
            layerNames[i] = layerVal.stringValue;
            
            if (layerNames[i] != oldLayerNames[i])
            {
                changedLayerName[i] = true;
                changeLayers = true;
            }
            else if (layerNames[i] == "Main Layer" && i == 20)
            {
                hasMain = true;
                changedLayerName[i] = false;
            }
            else if (layerNames[i] == "")
                changeLayers = true;
            else if (index == i && ln != oldLayerNames[i] && ln != "")
            {
                changedLayerName[i] = true;
                changeLayers = true;
                layerNames[i] = ln;
            }
            else changedLayerName[i] = false;
        }

        
        if (!hasMain)
            changeLayers = true;


        #endregion

        #region Altera as Layers

        if (changeLayers)
        {
            for (int i = 8; i <= Constants.maxLayerLenght-1; i++)
            {
                SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(i);      
                if (changedLayerName[i])
                {
                    oldLayerNames[i] = layerNames[i];
                    layerVal.stringValue = layerNames[i];
                }
                else if (layerNames[20] == "" && layerNames[20] != "Main Layer" && i == 20)
                {
                    if (!hasMain)
                    {
                        layerVal.stringValue = "Main Layer";
                        hasMain = true;
                    }
                }
                else if (layerNames[i] == "")
                    layerVal.stringValue = "Layer " + i;

            }
            changeLayers = false;
        }

        for (int i = 8; i < Constants.maxLayerLenght; i++)
        {
            SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(i);
            for (int j = 8; j < Constants.maxLayerLenght; j++)
            {
                if (oldLayerNames[i] == oldLayerNames[j] && j != i && oldLayerNames[i] != "")
                {
                    oldindex = index;
                    equalLayername = true;
                }
            }
            if (equalLayername && i == index)
            {
                oldLayerNames[i] = "Layer " + i + 100;
                layerVal.stringValue = oldLayerNames[i];
                newlayerName = layerVal.stringValue;
            }
        }

        if (equalLayername && oldindex != index)
            EditorUtility.DisplayDialog(Constants.errorString, "This layer name is already taken. The layer names was changed to: " + newlayerName, Constants.okString);
    
    #endregion

    //Salva
    tagManager.ApplyModifiedProperties();
    return (oldLayerNames);
    }

    #endregion

    #region Edita as Tags

    public string[] getTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            oldTagNames[i] = t.stringValue.ToString();
        }
        return oldTagNames;
    }

    #region EditTags

    public string[] editTag(string[] tagNames, string tn, int index)
    {
        bool editTag = false;
        bool tnIgual = false;
        bool[] changedTagName = new bool[Constants.maxTagLenght];

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        #region Busca tag alterada

        if(tn == "")
            EditorUtility.DisplayDialog(Constants.errorString, "The tag value in textbox cannot be null.", Constants.okString);
        else
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (tn == t.stringValue.ToString())
                    tnIgual = true;
            }
            if (!tnIgual)
            {
                for (int i = 0; i < tagsProp.arraySize; i++)
                {
                    SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                    tagNames[i] = t.stringValue.ToString();

                    if (oldTagNames[i] != tagNames[i])
                    {
                        editTag = true;
                        changedTagName[i] = true;
                    }
                    else if (index == i && tn != oldLayerNames[i])
                    {
                        editTag = true;
                        changedTagName[i] = true;
                        tagNames[i] = tn;
                    }
                    else changedTagName[i] = false;
                }

            }
        }
        #endregion

        #region Altera as Tags

        if (editTag && !tnIgual)
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty tag = tagsProp.GetArrayElementAtIndex(i);
                if (changedTagName[i] == true)
                {
                    oldTagNames[i] = tagNames[i];
                    tag.stringValue = tagNames[i];
                }

            }
            editTag = false;
        }
        else if (tnIgual)
            EditorUtility.DisplayDialog(Constants.errorString, "This tag name is already taken.", Constants.okString);

        //Salva
        tagManager.ApplyModifiedProperties();
        return (oldTagNames);
    }
    #endregion

    #endregion

    #region Remove Tags

    public string[] removeTag(string tagName)
    {
        bool tagEqual = false;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        //Busca a tag a ser apagada
        if (tagName == "")
            EditorUtility.DisplayDialog(Constants.errorString, "The tag value in textbox cannot be null.", Constants.okString);
        else
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                string tag = t.stringValue.ToString();

                if (tag == tagName)
                {
                    t.stringValue = "";
                    tagEqual = true;
                }
                oldTagNames[i] = t.stringValue.ToString();
            }
        }
        if(!tagEqual)
            EditorUtility.DisplayDialog(Constants.errorString, "This tag don't exist.", Constants.okString);
        //Salva
        tagManager.ApplyModifiedProperties();
        return (oldTagNames);
    }

    #endregion

    #region AddTags

    public string[] addTags(string tagName)
    {
        bool addTag = true;
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        //Checa para ver se não existe a Tag
        if (tagName == "")
            EditorUtility.DisplayDialog(Constants.errorString, "The tag value in textbox cannot be null.", Constants.okString);
        else
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                string tag = t.stringValue.ToString();

                if (tag == tagName)
                    addTag = false;
            }
        }

        if (addTag)
        {
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            SerializedProperty tag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            tag.stringValue = tagName;
        }
        else
            EditorUtility.DisplayDialog(Constants.errorString, "This tag name is already taken.", Constants.okString);
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            oldTagNames[i] = t.stringValue.ToString();
        }
        //Salva
        tagManager.ApplyModifiedProperties();
        return (oldTagNames);
    }

    #endregion

    #endregion

    #region Reset

    public void resetLayersAndTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = 8; i <= 31; i++)
        {
            SerializedProperty layerVal = layersProp.GetArrayElementAtIndex(i);
            layerVal.stringValue = "";
        }

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            t.stringValue = "";
            oldTagNames[i] = t.stringValue.ToString();
        }

        //Salvar
        tagManager.ApplyModifiedProperties();
    }

    #endregion

}
