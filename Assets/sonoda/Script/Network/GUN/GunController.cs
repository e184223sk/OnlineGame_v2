/* --------------------------------------------------
 ##銃の制御用クラス##
 
    銃だけでなくロケットランチャーでもなんでも管理可能な、なんか...
    素敵なクラス

 writting by ikeda
  -------------------------------------------------- */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public class GunController : MonoBehaviour
{
    [Space(10)]

    //============================================================================================-
    // 発射モード関連 ----------------------------------------------------------------------------|
    //============================================================================================-

    /// <summary>
    /// アクティブかどうか? 外部クラスの制御用
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// セミオートとかフルオートか? 
    /// </summary>
    public GunMode_Automatic GunModeAuto;
    

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
    /// 自動でリロードするか?
    /// </summary>
    public bool AutoReload; 

    [Space(10)]
    /// <summary>
    /// マガジンの最大数
    /// </summary>
    public int MaximumInMagazines;

    /// <summary>
    /// マガジンの残弾数
    /// </summary>
    public int RemainingInMagazines;


    /// <summary>
    /// 手持ちの最大数残弾数
    /// </summary>
    public int MaximumOnHand;

    /// <summary>
    /// 手持ちの残弾数
    /// </summary>
    public int RemainingOnHand;




    /// <summary>
    /// 弾丸の発射速度
    /// </summary>
    [Range(0,50), SerializeField] float fireSpeed = 1f;


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
            else if (fireSpeed > 50) fireSpeed = 50;
        }
    }



    /// <summary>
    /// リロード速度
    /// </summary>
    [Range(0, 20),SerializeField] float reloadSpeed = 1;


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


    /// <summary>
    /// 銃をリロード可能かを示す変数
    /// 例えば崖にしがみついてたり、被弾してたりなど
    /// 別のことをしてる時falseにするとリロードできなくなる
    /// </summary>
    public bool DoReload;

    [Range(1,50)]
    public float BulletSpeed = 2;

    //============================================================================================-
    // カプセル化のためアクセスできないやつ ----------------------------------------------------------------------------|
    //============================================================================================-
    protected Transform RejectCartridgePoint;  //排莢する場所のEmptyObject
    protected Transform BalletSpawnPoint;      //弾丸を発射する場所のEmptyObject
    ParticleSystem FireParticle;     //火薬の煙とか火を発射する場所のEmptyObject
    AudioSource FireSe;   //発射音を再生するAudioSource 
    AudioSource ReloadSe; //リロード時のガチャ的な音を再生するAudioSource
    float WaitFireCnt; //発射速度調整のカウントをする変数
    bool ISFIRE;       //打てるか否か !?
    bool IsReload;     //リロード中か !?
    float ReloadCnt; 



    //============================================================================================-
    // カプセル化のためアクセスできないやつ ----------------------------------------------------------------------------|
    //============================================================================================-
    [Space(20)]
    //弾丸のPrefab
    [SerializeField]protected string Bullet_Prefab;
    
    //薬莢のPrefab
    [SerializeField]protected string Reject_Prefab; 




    void Start()
    {
        RejectCartridgePoint = transform.Find("RejectCartridgePoint");
        BalletSpawnPoint = transform.Find("BalletSpawnPoint");
        FireSe = transform.Find("FireSe").GetComponent<AudioSource>();
        ReloadSe = transform.Find("ReloadSe").GetComponent<AudioSource>();
    }



    void Update()
    {
        if (!IsMine())
            return;
        Debug.Log("log - A");
        if (IsActive)
        {

            Debug.Log("log - B");
            if (RemainingInMagazines > 0)
            { 
                Debug.Log("log - C");
                WaitFireCnt += fireSpeed * Time.deltaTime;
                if (WaitFireCnt >= 1) WaitFireCnt = 0;
                ISFIRE = WaitFireCnt == 0;
                
                if (ISFIRE)
                {

                    Debug.Log("log - D");
                    if (GunModeAuto == GunMode_Automatic.FullAuto)
                    {

                        Debug.Log("log - E0");
                        if (FireKey_Full)
                            NetFire();
                    }
                    else
                    {
                        Debug.Log("log - E1");
                        if (FireKey_Semi)
                            NetFire();
                    }
                } 
            }


            //リロード関連 ----------------------------------
            if ( IsReload)
            { 
                ReloadCnt += Time.deltaTime * reloadSpeed;

                if (ReloadCnt > 1)
                {
                    if (RemainingOnHand > 0)
                    {
                        WaitFireCnt = 0;
                        if (RemainingOnHand >= MaximumInMagazines)
                        {
                            RemainingOnHand -= MaximumInMagazines;
                            RemainingInMagazines = MaximumInMagazines;
                        }
                        else
                        {
                            RemainingOnHand -= RemainingOnHand;
                            RemainingOnHand = 0;
                        }
                    }

                    IsReload = false;
                    ReloadCnt = 0;
                }
            }
            else
            {
                IsReload = FireKey_Reload;
            }
           
        }
    }


    void Fire()
    { 
        Debug.Log("log - F");
        RemainingInMagazines--;
        RemainingOnHand--;
        // FireSe.PlayOneShot(FireSe.clip);// 音
        // ReloadSe.PlayOneShot(ReloadSe.clip);// 音
        GameObject f1 = Instantiate(Resources.Load(Bullet_Prefab) as GameObject); //発射
        f1.transform.position = BalletSpawnPoint.position;
        f1.transform.rotation = BalletSpawnPoint.rotation;
        BulletController bc = f1.GetComponent<BulletController>();
        bc.force = new Vector3(bc.force.x, bc.force.y * BulletSpeed, bc.force.z); 

        GameObject f2 = Instantiate(Resources.Load(Reject_Prefab) as GameObject); //薬莢排出
        f2.transform.position = RejectCartridgePoint.position;
        f2.transform.rotation = RejectCartridgePoint.rotation;
        //火薬エフェクト
    }

     

    protected virtual bool IsMine()
    {
        return true;
    }

    protected virtual void NetFire() {}


    //キー入力関連
    bool FireKey_Semi { get { return Input.GetKeyDown(KeyCode.Q); } }
    bool FireKey_Full { get { return Input.GetKey(KeyCode.Q); } }
    bool FireKey_Reload { get { return Input.GetKeyDown(KeyCode.E); } }

}




/// <summary>
/// 銃がフルオートか? セミか？　的なクラス
/// </summary>
public enum GunMode_Automatic
{
    SemiAuto,
    FullAuto
}


