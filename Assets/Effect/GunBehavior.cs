/* --------------------------------------------------
 ##銃の制御用クラス##
 
    銃だけでなくロケットランチャーでもなんでも管理可能な、なんか...
    素敵なクラス

 writting by ikeda
  -------------------------------------------------- */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunBehavior : WeaponBehavior
{
    [Space(10)]

    public moverTES mover;
    public PLAYERS player;
    /// <summary>
    /// アクティブかどうか? 外部クラスの制御用
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// セミオートとかフルオートか? 
    /// </summary>
    public GunMode_Automatic GunModeAuto;

    /// <summary>
    /// 自動でリロードするか?
    /// </summary>
    public bool AutoReload;

    public MagazineData Loaded, Spare;
     

    public float FireScale;
    public LockOn_GunTarget gunTarget;
    /// <summary>
    /// 弾丸の発射速度
    /// </summary>
    [Range(0, 20), SerializeField] protected float fireSpeed = 1f;

    /// <summary>
    /// リロード速度
    /// </summary>
    [Range(0, 20), SerializeField] protected float reloadSpeed = 1;

    /// <summary>
    /// 銃をリロード可能かを示す変数
    /// 例えば崖にしがみついてたり、被弾してたりなど
    /// 別のことをしてる時falseにするとリロードや攻撃できなくなる
    /// </summary>
    public bool DoTASK;

    [Range(1, 100)]
    public float BulletSpeed = 2;
    [Range(1, 40)]
    public float LifeTime = 2;

    [Range(0, 3)]
    public float StartFireDelay;
    
    [Space(20)]
    [SerializeField] protected string Bullet_Prefab;//弾丸のPrefab 
    [SerializeField] protected string Reject_Prefab;   //薬莢のPrefab

    [SerializeField, Range(0, 1)] protected float ReloadCnt;

    [Header("ショットガン用のパラメータ")]
    [Range(1,20)]
    public int bulletsPerShot = 1;
    [Range(0.1f,2)]
    public float Radius = 1;

    bool semi__isFire;

    public bool IsSemiAuto
    {
        get
        {
            return GunModeAuto == GunMode_Automatic.SemiAuto;
        }
        set
        {
            GunModeAuto = value ? GunMode_Automatic.SemiAuto : GunMode_Automatic.FullAuto;
        }
    }


    public bool IsFullAuto
    {
        get
        {
            return GunModeAuto == GunMode_Automatic.FullAuto;
        }
        set
        {
            GunModeAuto = value ? GunMode_Automatic.FullAuto : GunMode_Automatic.SemiAuto;
        }
    }






    //============================================================================================-
    // マガジン関連 ----------------------------------------------------------------------------|
    //============================================================================================-


    /// <summary>
    /// 弾丸の発射速度 (プログラム操作用)
    /// </summary>
    public float FireSpeed
    {
        get
        {
            return fireSpeed;
        }
        set
        {
            fireSpeed = value;
            if (fireSpeed < 0) fireSpeed = 0;
            else if (fireSpeed > 20) fireSpeed = 20;
        }
    }




    /// <summary>
    /// リロード速度 (プログラム操作用)
    /// </summary>
    public float ReloadSpeed
    {
        get
        {
            return reloadSpeed;
        }
        set
        {
            reloadSpeed = value;
            if (reloadSpeed < 0) reloadSpeed = 0;
            else if (reloadSpeed > 20) reloadSpeed = 20;
        }
    }



    protected Transform RejectCartridgePoint;  //排莢する場所のEmptyObject
    protected Transform BalletSpawnPoint;      //弾丸を発射する場所のEmptyObject
    protected ParticleSystem FireParticle;     //火薬の煙とか火を発射する場所のEmptyObject
    protected AudioSource FireSe;   //発射音を再生するAudioSource 
    protected AudioSource ReloadSe; //リロード時のガチャ的な音を再生するAudioSource
    protected float WaitFireCnt; //発射速度調整のカウントをする変数
    protected bool ISFIRE;       //打てるか否か !?
    protected bool IsReload;     //リロード中か !?



    public float cnt_SFD;


    public void Start()
    {
        RejectCartridgePoint = transform.Find("RejectCartridgePoint");
        BalletSpawnPoint = transform.Find("BalletSpawnPoint");
        FireSe = transform.Find("FireSe").GetComponent<AudioSource>();
        ReloadSe = transform.Find("ReloadSe").GetComponent<AudioSource>();
        try
        {
            player = transform.root.GetComponent<PLAYERS>();
            mover = transform.root.GetComponent<moverTES>();
        }
        catch (System.Exception)
        {

        }
    }
     

    void Update()
    {
        if (IsActive && !DoTASK)
        {
            bool x = ((FireKey_Full && (GunModeAuto == GunMode_Automatic.SemiAuto)) || (FireKey_Full && (GunModeAuto == GunMode_Automatic.FullAuto)));

            mover.animator.SetLayerWeight(1, x ? 1 : 0);
            if (x != mover.animator.GetBool(animations))
                mover.animator.SetBool(animations, x);

            gunTarget.Look = x;
            gunTarget.LeftGun  = x && hand == HasHand.LEFT;
            gunTarget.RightGun = x && hand == HasHand.RIGHT;

            if (Loaded.now > 0 && !IsReload )
            { 
                if (GunModeAuto == GunMode_Automatic.FullAuto)
                {
                    if (x) cnt_SFD += Time.deltaTime;
                    else cnt_SFD = 0;
                    if (cnt_SFD >= 1)
                    {
                        WaitFireCnt += fireSpeed * Time.deltaTime;
                        if (WaitFireCnt >= 1) WaitFireCnt = 0;

                        if (WaitFireCnt == 0 && FireKey_Full) Fire();
                    }
                }
                else
                {
                    if (semi__isFire)
                    {
                        cnt_SFD += Time.deltaTime;
                        if (cnt_SFD > 1)
                        {
                            cnt_SFD = 0;
                            semi__isFire = false;
                            Fire();
                        }
                    }
                    else if (FireKey_Semi)
                        semi__isFire = true;  
                }
                // else if (!x) cnt_SFD = 0; 


                /*
             WaitFireCnt += fireSpeed * Time.deltaTime;
                if (WaitFireCnt >= 1) WaitFireCnt = 0;

                Debug.Log("b0");
                if (WaitFireCnt == 0) //カウントが1周したら...
                {
                    Debug.Log("b1");
                    if (GunModeAuto == GunMode_Automatic.FullAuto)
                    {
                        if (FireKey_Full) GO_Fire();
                        else cnt_SFD = 0;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            Debug.Log("FireKey_Semi is True");
                            GO_Fire();
                        }
                        else cnt_SFD = 0;
                    }
                }
               // else if (!x) cnt_SFD = 0; 
                        
             
             */
            }





            //リロード関連 ----------------------------------
            if (!IsReload && Loaded.now < Loaded.max && !FireKey_Semi && !FireKey_Full)
            {
                IsReload = FireKey_Reload;
                if (IsReload)
                {
                    if (ReloadSe.clip != null)
                        ReloadSe?.PlayOneShot(ReloadSe.clip);// 音
                }
            }


            if (IsReload)
            {
                ReloadCnt += Time.deltaTime * reloadSpeed;

                if (ReloadCnt > 1)
                {
                    WaitFireCnt = 0;
                    int y = (Loaded.max - Loaded.now < Spare.now) ? Loaded.max - Loaded.now : Spare.now;//装填弾数
                    Loaded.now += y;
                    Spare.now -= y;
                    IsReload = false;
                    ReloadCnt = 0;
                }
            }

        }
    }

     


    void Fire()
    {
        if (FireSe.clip != null) FireSe?.PlayOneShot(FireSe.clip);// 音
        if (Reject_Prefab != "")
        {
            GameObject f2 = Instantiate(Resources.Load("GUN/" + Reject_Prefab) as GameObject); //薬莢排出
            f2.transform.position = RejectCartridgePoint.position;
            f2.transform.rotation = RejectCartridgePoint.rotation;
        }
        EffectGenerator.CreateEffect(EffectType.GunFire, BalletSpawnPoint.position, BalletSpawnPoint.rotation).transform.localScale *= FireScale;
         


        bulletsPerShot = bulletsPerShot <= 0 ? 1 : bulletsPerShot;

        if (bulletsPerShot == 1) MakeBullet();
        else
            for (int v = 0; v < bulletsPerShot; v++)
                if (Loaded.now > 0)
                    MakeBullet().Rotate(Random.Range(Radius / 4 * -45, Radius / 4 * 45), 0, Random.Range(Radius / 4 * -45, Radius / 4 * 45));    
  
    }

    Transform MakeBullet()
    {
        Loaded.now--;
        GameObject f1 = Instantiate(Resources.Load("GUN/" + Bullet_Prefab) as GameObject); //発射
        f1.transform.position = BalletSpawnPoint.position;
        f1.transform.rotation = BalletSpawnPoint.rotation;

        BulletBehaviour bc = f1.GetComponent<BulletBehaviour>();
        bc.INIT(player.userID, LifeTime, BulletSpeed);

        f1.GetComponent<DamageObject>().playerStatus = player;
        return bc.transform;
    }


    //キー入力関連
    bool FireKey_Semi   => Key.B.Down;
    bool FireKey_Full   => Key.B.Press;
    bool FireKey_Reload => Key.X.Down;


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (transform.Find("BalletSpawnPoint") == null)
        {
            var c = new GameObject("BalletSpawnPoint").transform;
            c.parent = transform;
            c.localPosition = Vector3.zero;
        }


        if (transform.Find("RejectCartridgePoint") == null)
        {
            var c = new GameObject("RejectCartridgePoint").transform;
            c.parent = transform;
            c.localPosition = Vector3.zero;
        }

        Start();
        if (RejectCartridgePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(RejectCartridgePoint.position, 0.05f);
            Gizmos.DrawLine(RejectCartridgePoint.position, RejectCartridgePoint.position + RejectCartridgePoint.up * 0.4f);
        }

        Gizmos.color = new Color(1f, 0.6f, 0);
        Gizmos.DrawWireCube(transform.position, new Vector3(0.04f, 0.1f, 0.04f));

        if (Loaded.now > Loaded.max) Loaded.now = Loaded.max;
        if (Spare.now > Spare.max) Spare.now = Spare.max;

        if (BalletSpawnPoint != null)
        {
            if (bulletsPerShot > 1)
            {
                Gizmos.color = Color.red;
                for (int v = 0; v < 12; v++)
                {
                    Vector3 vps = new Vector3(Mathf.Sin(2 * Mathf.PI / 12 * v), 0, Mathf.Cos(2 * Mathf.PI / 12 * v)) * Radius;
                    Vector3 vns = new Vector3(Mathf.Sin(2 * Mathf.PI / 12 * (v + 1)), 0, Mathf.Cos(2 * Mathf.PI / 12 * (v + 1))) * Radius;
                    Vector3 FP = BalletSpawnPoint.up * vps.y + BalletSpawnPoint.forward * vps.z + BalletSpawnPoint.right * vps.x + BalletSpawnPoint.up * LifeTime * BulletSpeed;
                    Vector3 FN = BalletSpawnPoint.up * vns.y + BalletSpawnPoint.forward * vns.z + BalletSpawnPoint.right * vns.x + BalletSpawnPoint.up * LifeTime * BulletSpeed;
                    Gizmos.DrawLine(BalletSpawnPoint.position, BalletSpawnPoint.position + FP);
                    Gizmos.DrawLine(BalletSpawnPoint.position + FP, BalletSpawnPoint.position + FN);
                }
            }
            else
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(BalletSpawnPoint.position, 0.1f);
                Gizmos.DrawLine(BalletSpawnPoint.position, BalletSpawnPoint.position + BalletSpawnPoint.up * LifeTime * BulletSpeed);
                Gizmos.DrawWireSphere(BalletSpawnPoint.position + BalletSpawnPoint.up * LifeTime * BulletSpeed, 0.05f);
            }
        }
        
    }
#endif

    public virtual void GIZMOS() => Debug.Log("not override Method \"GIZMOS\"");
    public virtual void UPDATE() => Debug.Log("not override Method \"UPDATE\"");
    public virtual void START () => Debug.Log("not override Method \"START\"");
}



/// <summary>
/// 銃がフルオートか? セミか？　的なクラス
/// </summary>
public enum GunMode_Automatic
{
    SemiAuto,
    FullAuto
}




[System.Serializable]
public class MagazineData
{
    public int now, max;
}



/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
//[RequireComponent(typeof(Au))]
public class GunBehavior : MonoBehaviour
{
    protected enum ActionMode { FullAuto, SemiAuto };
    protected delegate void GUN_Event();
    protected delegate void GUN_DelayEvent(float v);

    [SerializeField]
    public MagazineData magazines;

    protected ActionMode actionMode;
    public string   bulletName;
    public string   fireEffectName;
    public string   exhaustName;
    protected bool  IsReload;
    protected float fullAutoIntervalCnt;
    protected float fullAutoInterval;
    protected float FireDelayCnt;
    protected float FireDelay;
    protected bool  FireFlag;

    protected GUN_DelayEvent fireDelayEvent;
    protected GUN_Event fireTime;
    protected GUN_Event flagUp;
    protected GUN_Event flagDown;
    protected GUN_Event reloadingTime;
    protected GUN_Event reloadStart;
    protected GUN_Event reloadFinished;

    AudioSource se;
    AudioSource reload;

    AudioClip fireSe;
    

    void Start()
    {
        
    }
    

    void Update()
    {
        if (IsReload)
        {
            reloadingTime?.Invoke();
            magazines.time += Time.deltaTime;
            if (magazines.time > magazines.reloadTime)
            {
                reloadFinished?.Invoke();
                magazines.spare -= magazines.max;
                if (magazines.spare < 0) magazines.spare = 0;
                magazines.now = magazines.max;
                magazines.time = 0;
            }
        }
        if (magazines.now > 0)
        { 
            if (actionMode == ActionMode.FullAuto)
            {

                if (Input.GetKeyDown(KeyCode.A))
                { 
                    fullAutoIntervalCnt += Time.deltaTime;
                    if (fullAutoIntervalCnt > fullAutoInterval)
                    {
                        Fire();
                        fullAutoIntervalCnt = 0;
                    }
                }
            }
            else
            {
                if (!FireFlag)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                        FireFlag = true;
                }
                else
                {
                    FireDelayCnt += Time.deltaTime;
                    if (FireDelayCnt > FireDelay)
                    {
                        FireDelayCnt = 0; 
                        Fire();
                    }
                }
            }
        }
    }


    void Fire()
    {
        //火花
        //排莢
        //銃
    }

    public void Reload()
    {
        IsReload = magazines.spare >= magazines.max;
        if (IsReload) reloadStart?.Invoke();
    }
}



 */
