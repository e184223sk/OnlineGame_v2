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


    public PLAYERS playerStatus;

    public void Set(PLAYERS p)
    {
        playerStatus = p;
    }

    void OnTriggerStay(Collider c)
    { 
        if (IsDamage)
        { 
            var tp = c.transform.GetComponent<PLAYERS>();
            if(tp == null) tp = c.transform.root.GetComponent<PLAYERS>();
            if (tp != playerStatus)
            {
                Debug.Log("a1");
                var ee = c.transform.root.gameObject; 
                if (IsDestroy)
                {
                    Penetration -= Time.deltaTime;
                    if (Penetration <= 0)
                        Destroy(gameObject);
                }
                if (tp != null)
                {
                    Debug.Log(playerStatus == tp ? "PS=tp" : "PS!=tp");
                    Debug.Log("a4");
                    Debug.Log("hit damage __ " + tp.transform.root.name);
                    Debug.Log("ax" + (Damages - tp.DefensePoint));
                    tp.HP -= Damages - tp.DefensePoint;
                    if (tp.HP < 0) tp.HP = 0;
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



