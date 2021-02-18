using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{ 
    public Vector3 force;
    public float timer;
    public bool DoUpdate = true;
    public Space mode;
    public float Gravity = 1;
    protected void Init(float v, Space m)
    {
        mode = m;
        Destroy(gameObject, v); 
    }

    void Update()
    {
        if (!DoUpdate) return;
        timer += Time.deltaTime;
        UPDATE();
        transform.Translate(force * Time.deltaTime, mode);
        transform.Translate(Gravity * Time.deltaTime * Vector3.down, Space.World);
    }

    public virtual void UPDATE() { }
}