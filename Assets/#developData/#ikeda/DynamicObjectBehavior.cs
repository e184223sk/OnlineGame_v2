using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class DynamicObjectBehavior : MonoBehaviour
{ 
#if UNITY_EDITOR

   void OnDrawGizmos()
   {
       foreach (Transform c in gameObject.transform)
       {
           if (c.gameObject.GetComponents<Collider>() != null)
               if (c.gameObject.GetComponent<DynamicObject>() == null)
               {
                    
                    c.gameObject.AddComponent<DynamicObject>();
                    var r = c.gameObject.AddComponent<Rigidbody>();
                    if (r != null)
                    {
                        DestroyImmediate(r);
                    }
                    //r.isKinematic = true;
                    //r.useGravity = false;
                    //r.constraints = RigidbodyConstraints.FreezeAll; 
                }
       }
   }

#endif

}

public class DynamicObject : MonoBehaviour
{
    public float durability = 100;
    public void OnCollisionStay(Collision c)
    {
        Debug.Log("OnCollisionStay");
        if (c.transform.root != transform.root)
        {
            durability -= c.impactForceSum.magnitude;
            if (durability <= 0)
            {
                c.transform.parent = null;
                var r = c.gameObject.AddComponent<Rigidbody>();
                r.isKinematic = false;
                r.useGravity = true;
                r.constraints = RigidbodyConstraints.None;
                //同期オブジェクトの処理を追加
            }
        }
    }
}