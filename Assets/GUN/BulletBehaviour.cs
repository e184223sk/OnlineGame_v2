using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    string n; //ID
    public float Speed,Gravity; //速度、重力
    protected float LifeTime = 4; //寿命
    protected float c;//時間のカウント処理に使用

    public void INIT(string n, float LifeTime, float Speed)
    { 
        this.n = n;
        this.LifeTime = LifeTime;
        this.Speed = Speed;
    }

    public void Update()
    {
        c += Time.deltaTime;
        if (LifeTime < c)
        {
            Destroy(gameObject); 
        }

        UPDATE();

        transform.Translate(Speed * Vector3.up * Time.deltaTime, Space.Self);
        transform.Translate(Gravity * Vector3.down * Time.deltaTime, Space.World);
    }

    public void OnTriggerStay(Collider co)
    {
       // if (c < 0.1f) return;
        //GunBehavior gb = co.transform.root.GetComponent<GunBehavior>();
        //if (gb == null) gb = co.transform.GetComponent<GunBehavior>();

        PLAYERS pl = co.transform.root.GetComponent<PLAYERS>();
        if (pl == null) pl = co.transform.GetComponent<PLAYERS>();

        BulletBehaviour bb = co.transform.root.GetComponent<BulletBehaviour>();
        if (bb == null) bb = co.transform.GetComponent<BulletBehaviour>();

        if (bb != null)
        { 
            Debug.Log("弾丸同士の接触");
            Destroy(gameObject); 
            return;
        }

        if (pl == null/* && gb == null*/)
            HittingObject(co, false);
        else if (pl != null /*&& gb != null*/ && pl.userID != n /*&& gb.userID != n*/)
            HittingObject(co, true);
    }

    public virtual void UPDATE() { }
    public virtual void HittingObject(Collider collision, bool IsCreature) { Debug.Log("call  base.HittingObject"); }
}
 