using UnityEngine;

public class MissileBehavior : BulletBehaviour
{
    float c;
    void Start() => GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    public override void UPDATE() => c += Time.deltaTime;

    public override void HittingObject(Collider collision, bool k)
    {
        if (c < 0.05f) return;
        EffectGenerator.CreateEffect(EffectType.Explosion_3x3, transform.position);
        Destroy(gameObject);
    }
}