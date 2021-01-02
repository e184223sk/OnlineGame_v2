using UnityEngine; 
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lang_JpAttribute : PropertyAttribute
{
    public readonly string tag_;
    public Lang_JpAttribute(string x) { tag_ = x; }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Lang_JpAttribute))]
public class LabelAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    { 
        EditorGUI.PropertyField(position, property, new GUIContent((attribute as Lang_JpAttribute).tag_), true);
    }

    public override float GetPropertyHeight(SerializedProperty x, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(x, true);
    }
}
#endif