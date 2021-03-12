using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageObject : MonoBehaviour
{
    public bool  IsDamage;
    public DamageData Damages;

    [Space(10)]
    public bool  IsDestroy;
    public float Penetration = 3;
     

    PLAYERS playerStatus;

    public void Set(PLAYERS p)
    {
        playerStatus = p;
    }

    void OnTriggerStay(Collider c)
    {
        Debug.Log("A:" + c.transform.root.gameObject.name);
        if (IsDamage)
        { 
            Debug.Log("B:" + c.transform.root.gameObject.name);
            var tp = c.transform.root.GetComponent<PLAYERS>();
            if (tp != playerStatus)
            {

                Debug.Log("C:" + c.transform.root.gameObject.name);
                if (tp != null)
                {

                    Debug.Log("D:" + c.transform.root.gameObject.name);
                    tp.HP -= Damages - tp.DefensePoint;  
                }


                if (IsDestroy)
                {
                    RaycastHit hit;
                    var f = transform.position - c.transform.position;
                    Ray ray = new Ray(f, Vector3.Cross(f, c.transform.position));
                    if (c.Raycast(ray, out hit, 30f))
                    {
                     //   Penetration -= Vector3.Distance(hit.point, c.transform.position) * 2;
                        if (Penetration <= 0)
                            Destroy(gameObject);
                    }
                }

            } 
        }

    } 
}
 
[System.Serializable]
public class DamageData
{
    public float Slash; 
    public float Batting;
    public float Shooting;
    public float Bombing;
    public float Fire;

    public static float operator -(DamageData ap, DamageData dp)
    {
        float v = 0;
        v += ap.Slash    > dp.Slash    ? ap.Slash    - dp.Slash    : 0;
        v += ap.Batting  > dp.Batting  ? ap.Batting  - dp.Batting  : 0;
        v += ap.Shooting > dp.Shooting ? ap.Shooting - dp.Shooting : 0;
        v += ap.Bombing  > dp.Bombing  ? ap.Bombing  - dp.Bombing  : 0;
        v += ap.Fire     > dp.Fire     ? ap.Fire     - dp.Fire     : 0;
        return v;
    }

}



