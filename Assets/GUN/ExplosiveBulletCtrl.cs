using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBulletCtrl : BulletBehaviour
{
    [SerializeField]
    float Delay = 3;

    [SerializeField]
    string ResourceName = "";
    int v;


    public override void HittingObject(Collider c, bool k) => v = 1;


    void Start()
    {
        switch (v)
        {
            case 0: break;

            case 1:
                Delay -= Time.deltaTime;
                v = 2;
                break;

            default:
                Instantiate(Resources.Load(ResourceName), transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;

        }
    }

}
