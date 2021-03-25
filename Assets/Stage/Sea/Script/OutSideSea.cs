using UnityEngine;

public class OutSideSea : MonoBehaviour
{
    public Transform VectorCtrlPointer;
    public Transform Sea0, Sea1, Sea2, Sea3, Sea4, Sea5, Sea6, Sea7;
    public float Distance;
#if UNITY_EDITOR
    void OnValidate()
    {
        if (!UnityEditor.EditorApplication.isPlaying) Start();  
    }
#endif

    void Start()
    {
        Vector3 p = Wave.wave.transform.position + Vector3.up * 0.1f;
        Vector3 c = Vector3.one * Wave.wave.GetScale* Distance;
        Sea0.position = new Vector3(-c.x, p.y, -c.z);
        Sea1.position = new Vector3(-c.x, p.y,  p.z);
        Sea2.position = new Vector3(-c.x, p.y,  c.z);
        Sea3.position = new Vector3( c.x, p.y, -c.z);
        Sea4.position = new Vector3( c.x, p.y,  p.z);
        Sea5.position = new Vector3( c.x, p.y,  c.z);
        Sea6.position = new Vector3( p.x, p.y, -c.z);
        Sea7.position = new Vector3( p.x, p.y,  c.z); 
    } 

}
