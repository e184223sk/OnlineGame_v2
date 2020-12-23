#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Editor_SetTag : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void EDITOR_SET()
    { 
        EditorApplication.hierarchyWindowChanged += delegate ()
        { 
            SetTag_("Ground", "GroundObject");
            Set_Static("GroundObject", true);
            Set_Static("Sea", false);
        };
    }


    static void Set_Static(string rootObj, bool Static = true)
    {  
        GameObject.Find(rootObj).isStatic = Static;
        foreach (Transform c in GameObject.Find(rootObj).transform)
            c.gameObject.isStatic = Static;
    }

    static void SetTag_(string tagName, string rootObj)
    {
        if (!ExistTag(tagName)) AddTag(tagName); 
        GameObject.Find(rootObj).tag = tagName;
        foreach (Transform c in GameObject.Find(rootObj).transform) c.tag = tagName;
    }

    static bool ExistTag(string tagname)
    {
        var x = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((x != null) && (x.Length > 0)) 
            using (var tags = new SerializedObject(x[0]).FindProperty("tags"))
                for (int i = 0; i < tags.arraySize; ++i)
                    if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                        return true;  
        return false;
    }

    static void AddTag(string tagname)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                {
                    return;
                }
            }

            int index = tags.arraySize;
            tags.InsertArrayElementAtIndex(index);
            tags.GetArrayElementAtIndex(index).stringValue = tagname;
            so.ApplyModifiedProperties();
            so.Update();
        }
    }
}
#endif