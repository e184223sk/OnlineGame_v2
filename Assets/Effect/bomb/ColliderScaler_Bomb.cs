using UnityEngine;

public class ColliderScaler_Bomb : MonoBehaviour
{
    public AnimationCurve Radius;
    SphereCollider col;
    [Range(0f,1f)]public float t;

    void Start()
    {
        col = GetComponent<SphereCollider>();
        t = 0;
    }

    void OnDrawGizmos()
    {
        Start();
        col.radius = Radius.Evaluate(t);
    }

    void Update()
    {
        t += Time.deltaTime;
        col.radius = Radius.Evaluate(t);
    }

}
