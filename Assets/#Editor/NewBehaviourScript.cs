using UnityEditor;
using UnityEngine;
public class Buildings : EditorWindow
{
    [MenuItem("CustomMenu/Example")]
    static void Example() => EditorWindow.GetWindow<Buildings>();
    GameObject g;
    Vector3 min, max;
    void OnGUI()
    {
        g = EditorGUILayout.ObjectField(g, typeof(GameObject)) as GameObject;
        min = EditorGUILayout.Vector3Field("min",min);
        max = EditorGUILayout.Vector3Field("max", min);
        if (GUILayout.Button("ボタン"))
        {
            Vector3 cc = new Vector3
                (
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
            Instantiate(g, cc, Quaternion.identity);
            Debug.Log("押された！");
        }
    }
}
 
