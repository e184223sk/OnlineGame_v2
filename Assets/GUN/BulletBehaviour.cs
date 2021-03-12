using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    string n;
    public float Speed,Gravity;
    protected float LifeTime = 4;
    protected float c;
    public void INIT(string n, float LifeTime, float Speed)
    {
        Debug.Log("FIRE!!");
        this.n = n;
        this.LifeTime = LifeTime;
        this.Speed = Speed;
    }

    public void Update()
    {
        c += Time.deltaTime;
        if (LifeTime < c)
            ;// Destroy(gameObject);
        UPDATE();
        transform.Translate(Speed * Vector3.up * Time.deltaTime, Space.Self);
        transform.Translate(Gravity * Vector3.down * Time.deltaTime, Space.World);
    }

    public void OnTriggerStay(Collider collision)
    {
        if (c < 0.1f) return;
        if 
        (
            collision.transform.root.name != n ||
            collision.transform.root.GetComponent<BulletBehaviour>() == null ||
            collision.transform.GetComponent<BulletBehaviour>() == null ||
            collision.transform.root.GetComponent<BulletBehaviour>().n != n ||
            collision.transform.GetComponent<BulletBehaviour>().n != n
        )
        {
            HittingObject(collision);
        }
    }

    public virtual void UPDATE() { }
    public virtual void HittingObject(Collider collision) { Debug.Log("call  base.HittingObject"); }
}