using UnityEngine;

public class BulletCtrl : BulletBehaviour
{
     
    public override void HittingObject(Collider collision)
    {
        if (collision.transform.root.transform.GetComponent<PLAYERS>())
        {
            EffectGenerator.CreateEffect(EffectType.GunDamageBlood, collision.ClosestPoint(transform.position)).transform.LookAt(transform.position);
        }
        else if(collision.transform.root.transform.GetComponent<EffectLimitter>() == null)
        {
            //    var r = (Instantiate(Resources.Load("GUN/holes"), collision.ClosestPoint(transform.position), Quaternion.identity) as GameObject);
            //    r.transform.rotation = transform.rotation;
            //    r.GetComponent<EffectLimitter>().SetLifeTime(5);
        }
        Destroy(gameObject);
    }

}
