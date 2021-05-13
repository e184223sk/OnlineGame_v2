using UnityEngine;

public class RejectBulletController : BulletBehaviour
{
    float v;
    Vector3 moveXZ;
    public float bounce,raising;
    public Vector3 rotationPower;
    const float PI = 2 * Mathf.PI;
    Vector3 direction;

    public void INIT(float cc)
    {
        transform.Rotate(Random.Range(cc / 4 * -45, cc / 4 * 45), 0, Random.Range(cc / 4 * -45, cc / 4 * 45));
    }

    void Start()
    {
        INIT("",1.5f,0);
        var x = Random.Range(0f, 2f);
        moveXZ = new Vector3 ( Mathf.Sin(x * PI), 0, Mathf.Cos(x * PI));

        direction = transform.forward;
    }

    public override void UPDATE()
    {
        v += Time.deltaTime; 
        float vv = Mathf.Pow(Mathf.Cos((v > 0.5f ? 0 : v) / 2), 2);  
        transform.rotation *= Quaternion.Euler(rotationPower * 180 * Time.deltaTime); 
        transform.position += Time.deltaTime * ((Vector3.Scale(direction, moveXZ) * bounce * vv)/*横はね処理*/ +  (Vector3.down * (Gravity - vv * raising))/*落下/跳ね上がり処理*/);
    }
}
