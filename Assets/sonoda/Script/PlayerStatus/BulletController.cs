using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : BulletBehaviour 
{
    void Start()
    {
        Init(5, Space.Self); 
    }

    public override void UPDATE()
    {
        force *= 0.99f;
        transform.Rotate(Vector3.right * Time.deltaTime * 0.01f, Space.Self);

         if (force.y < 0.1f)
            Destroy(gameObject);
        //timerで動作
        //DoUpdate
        //force
    }

    void OnCollisionStay(Collision c)
    {
       // force = Vector3.zero;
    }

}
