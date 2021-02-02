using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TANK : MonoBehaviour
{
    Rigidbody rigidbody_;
    [SerializeField, Range(0, 1000000)]
    float forwardSpeed;

    float L_Speed, R_Speed;

    IsCaterpillarGrounding CL, CR;
    [System.NonSerialized]
    public Transform Turret, Cannon;
    public tank_Range CannonRange;
    public tank_weaponloader CannonLoding;

    public CaterpillarData caterpillar_L, caterpillar_R;
    public float TurretSpinSpeed;
    public float Torque;
    float tss, tsx;

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
        rigidbody_.drag = 0;
        rigidbody_.angularDrag = 0;
        Turret = transform.Find("Body/Turret");
        Cannon = transform.Find("Body/Turret/MainCannon"); 
        caterpillar_L.Init(transform.Find("Body/CaterpillarL"));
        caterpillar_R.Init(transform.Find("Body/CaterpillarR"));
        CL = transform.Find("Body/CaterpillarL").GetComponent<IsCaterpillarGrounding>();
        CR = transform.Find("Body/CaterpillarR").GetComponent<IsCaterpillarGrounding>();
    }

    void Update()
    { 
        //------------------------------------------------------------
      //  tss = Key.FL;
    
        
        tsx += Time.deltaTime * Key.JoyStickR.Get.y;


        L_Speed = Key.JoyStickL.GetRAW.y;
        R_Speed = Key.JoyStickR.GetRAW.y;
        bool FMC = Key.A.Down;
        bool FSC = Key.B.Down;
        //--------------------------------------------------------------


        if (L_Speed > 1) L_Speed = 1; else if (L_Speed < -1) L_Speed = -1;
        caterpillar_L.Update(L_Speed);

        if (R_Speed > 1) R_Speed = 1; else if (R_Speed < -1) R_Speed = -1;
        caterpillar_R.Update(-R_Speed);

        Turret.Rotate(Vector3.up * Time.deltaTime * TurretSpinSpeed * tss * 360);

        if (tsx < 0) tsx = 0; else if (tsx > 1) tsx = 1;
        Cannon.localRotation =Quaternion.Euler(CannonRange.min + tsx * (CannonRange.max - CannonRange.min), 0, 0);


        var p = forwardSpeed * Time.deltaTime;//下記2行で使用する変数
        if (CL.IsGround)
        {
            rigidbody_.AddForceAtPosition(L_Speed  * p* (transform.forward + transform.right), caterpillar_R.Mesh.transform.position, ForceMode.Impulse);
        }
        if (CR.IsGround)
        {
            rigidbody_.AddForceAtPosition(-R_Speed  * p * (transform.forward - transform.right), caterpillar_L.Mesh.transform.position, ForceMode.Impulse);  
        }
        if (FMC)
        {
            Debug.Log("fire");
        }

        if (FSC)
        {
            Debug.Log("sub fire");
        }


    }
}

[System.Serializable]
public class CaterpillarData
{
    public float Radius;
    public float SpeedSensiviry = 1;
    public uint WheelCnt;
    public Transform[] Wheels;
    public MeshRenderer Mesh;

    public void Init(Transform root)
    {
        Mesh = root.GetComponent<MeshRenderer>();
        Wheels = new Transform[WheelCnt];
        for (int y = 0; y < WheelCnt; y++)
            Wheels[y] = root.Find("wheel"+y);
    }

    public void Update(float c)
    {
        foreach (var t in Wheels)
            t.Rotate(Vector3.up*Time.deltaTime*Radius* -c * SpeedSensiviry);
        Mesh.materials[0].mainTextureOffset += new Vector2(0, -c * SpeedSensiviry);
    }
}

[System.Serializable]
 public class tank_Range
{
    public float min, max;
}


[System.Serializable]
public class tank_weaponloader
{
    public float now, time;
    public bool IsReadyFire;
}

//発射|サブ|移動|se