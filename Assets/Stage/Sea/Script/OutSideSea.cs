using UnityEngine;

public class OutSideSea : MonoBehaviour
{
    public Transform Sea0, Sea1, Sea2, Sea3, Sea4, Sea5, Sea6, Sea7;

    void OnDrawGizmos() => Update();

    void Update()
    {
        Vector3 p = Wave.wave.transform.position;
        Vector3 c = Vector3.one * Wave.wave.GetScale;
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
