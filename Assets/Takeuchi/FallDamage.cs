using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour
{

    public GameObject HP_object = null;    //使うTextオブジェクト
    public int HP_num = 1000;   //HPの量（初期値100）
    private int Damage = 0; //ダメージの量
    public int PlayerY = 0; //ジャンプした時点でのY座標
    public int nowPlayerY = 0;  //現在のY座標
    public int DamageM = 1; //ダメージ倍率
    public bool NoDamage = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //nowPlayerYに現在のY座標を入れ続ける
        Vector3 Player = GameObject.Find("unitychan").transform.position;
        nowPlayerY = (int)Player.y;
        //Unity上で指定したTextオブジェクトからTextコンポーネントを取得
        Text HP_text = HP_object.GetComponent<Text>();

        //テキストに現在のHPを表示させる
        HP_text.text = "HP:" + HP_num;

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")   //ジャンプした時
        {
            //ジャンプ時点のY座標をPlayerYに保存
            Vector3 Player = GameObject.Find("unitychan").transform.position;
            PlayerY = (int)Player.y;
            //Debug.Log("ジャンプ");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") //着地した時
        {
            animator.SetBool("Ground_Exit", true);
            //落下ダメージがあるか判定
            if (NoDamage == false)
            {
                FDamageJud();
            }
            //ダメージ分HPを減らす
            HP_num -= Damage;
            Debug.Log(Damage+"のダメージを受けた");  //判定の確認用（消して大丈夫です）
            //ダメージを初期値に戻す
            Damage = 0;
        }
        animator.SetBool("Ground_Exit", false);
    }

    void FDamageJud()   //落下ダメージの判定
    {
        int Jud = PlayerY-nowPlayerY;
        if (Jud > 2)    //ジャンプした時点のY座標と現在のY座標の差が"2"以上のときダメージが入る（""内は全体を見て調整）
        {
            Damage = Jud/2*DamageM;
        }
    }
}