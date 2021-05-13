using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverTES : MonoBehaviour
{ 
    public bool IsCtrl;
    public bool DoWalk;

    [Space(100)]
    [SerializeField]
    Transform UsingCamera;
    public Animator animator;
    Rigidbody rigid;
    [Space(100)]

    public float moveSpeed = 0.16f;
    public float spinSpeed = 6.60f;
    public float runSpeed = 0.40f; 
    public float failSpeed;
    public float Gravity = 0.6f;
    public float massPoint;

    public bool IsJumpping = false;
    public float jumppingCnt;
    public float jumpPower;
    public float GroundingThreshold;
    public float FallDamageThreshold;
    Vector3 failedPoint;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    } 

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<ConectionBehavior>()._monobitView.isMine)
            return;

        if (UsingCamera == null)
        {
            UsingCamera = GameObject.Find("Camera").transform;
            UsingCamera.GetComponent<CameraMove>().target = transform;
        }
        rigid.centerOfMass = Vector3.up * massPoint;
        if (!IsCtrl)//ユーザ操作不能時
        {

            return;
        }

        //キー検知 ==========================================================
        var c = (Vector2)Key.JoyStickL;
        var r = (Vector2)Key.JoyStickR;
        var jamp = Key.A.Down;
        var sq = Key.B.Down;

        //ここで各動作の可否を決定する。
        DoWalk = true;



        //処理
        if (Gravity > 0) rigid.position -= (Vector3.down * Gravity * Time.deltaTime); //重力(rigidBodyでも入れているが浮いてしまうので)

        if (DoWalk)
        { 
            Vector3 i = ((Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2) && (UsingCamera != null)) ?
                   Key.JoyStickL.Get.y * Vector3.Scale(UsingCamera.transform.forward, new Vector3(1, 0, 1)).normalized * moveSpeed + Key.JoyStickL.Get.x * UsingCamera.right * moveSpeed :
                   Vector3.zero;
            rigid.position += new Vector3(i.x, 0, i.z) * Time.deltaTime;
            if (new Vector3(i.x, 0, i.z).sqrMagnitude > 0.001f)
                rigid.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(i.x, 0, i.z), spinSpeed * Time.deltaTime, 0));
            animator?.SetBool("_WALK_", i.magnitude > 0.001f);
        }

        if (IsJumpping)
        {
            jumppingCnt -= Time.deltaTime;

            if (jumppingCnt <= 0)
            {
                jumppingCnt = 0;
            }
            else
            {
                rigid.position += Vector3.up * Time.deltaTime * Mathf.Cos((1 - (jumppingCnt / 2))/2)* jumpPower;
            }
             
            RaycastHit hit;

            if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, GroundingThreshold) && jumppingCnt < 1.5f)
            {
                IsJumpping = false;
                transform.position = hit.point;
                rigid.position = hit.point;
                failedPoint = transform.position;
            }

            //着地判定と
        }
        else if (jamp)
        {
            IsJumpping = true;
            jumppingCnt = 2;
            failedPoint = transform.position;
        }


        RaycastHit h;

        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out h, 1))
        {
            float vv = failedPoint.y - transform.position.y;
            if (vv > FallDamageThreshold)
            {
                float damage = vv - FallDamageThreshold;
              //  GetComponent<PLAYERS>().HP -= damage;
            } 

            failedPoint = transform.position;
        }
        else if(transform.position.y > failedPoint.y)
        {
            failedPoint.y = transform.position.y;
        }

    }
}
