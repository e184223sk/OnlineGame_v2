using UnityEngine;

public class RejectBulletController : BulletBehaviour
{
    float v;
    Vector3 z;
    public float bounce,raising;
    const float PI = 2 * Mathf.PI;

    void Start()
    {
        INIT("", 0.5f,0);
        var x = Random.Range(0f, 2f);
        z = new Vector3 ( Mathf.Sin(x * PI), 0, Mathf.Cos(x * PI));    
    }

    public override void UPDATE()
    {
        v += Time.deltaTime;
        transform.Translate (z *(v > 0.5 ? 0: v) * bounce * Time.deltaTime, Space.Self);
        transform.Translate(Vector3.down * ((v > 0.5f ? 0: (Mathf.Sin(v) * raising)) -  Gravity) * Time.deltaTime, Space.World);
    }
}
