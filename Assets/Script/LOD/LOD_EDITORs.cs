#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LOD))]//拡張するクラスを指定
public class LOD_EDITORs : Editor
{ 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Button"))
        {
            var t = (target as LOD).transform; 
            RaycastHit hit; 
            if (Physics.Raycast(new Ray(t.position- Vector3.down  * -10, Vector3.down), out hit, Mathf.Infinity))
                t.position = hit.point;
        }
    }

}
#endif