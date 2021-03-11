using UnityEngine;

public class ColliderScaler_Bomb40x40 : MonoBehaviour
{
    [Range(1, 50000),Space(10)]
    public float RV;
    public AnimationCurve Radius;

    [Range(1, 50000), Space(10)]
    public float RX;
    public AnimationCurve RadiusPX;

    [Range(1, 50000), Space(10)]
    public float RY;
    public AnimationCurve RadiusPY;
    SphereCollider col;
    BoxCollider ccol;
    [Range(0f, 60f)] public float t;

    void Start() { t = 0; INIT(); } 
    void OnDrawGizmos() { INIT(); FixedUpdate(); }
    void Update() => t += Time.deltaTime;

    void INIT()
    {
        col = GetComponent<SphereCollider>();
        ccol = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        ccol.size = new Vector3(RX * RadiusPX.Evaluate(t), RX * RadiusPX.Evaluate(t), -RY * RadiusPY.Evaluate(t));
        col.radius  = -RV * Radius.Evaluate(t);
    }
}
