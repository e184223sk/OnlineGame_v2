using UnityEngine;

public class BulletCtrl : BulletBehaviour
{
    Vector3 point;
    private void Start()
    {
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        point = transform.position;
    }

    public override void HittingObject(Collider collision, bool k)
    {
        Debug.Log("call override hit methods!!");
        Vector3 ww = point;
        if (k)
        {
            Debug.Log("create blood splash");
            EffectGenerator.CreateEffect(EffectType.GunDamageBlood, collision.ClosestPoint(transform.position)).transform.LookAt(ww);
        }
        else
        {
            Debug.Log("create holes");
            var r = (Instantiate(Resources.Load("GUN/holes"), collision.ClosestPoint(transform.position), Quaternion.identity) as GameObject);
            r.transform.LookAt(ww);
            r.GetComponent<EffectLimitter>().SetLifeTime(5);
        }
        Destroy(gameObject);
    }

}
