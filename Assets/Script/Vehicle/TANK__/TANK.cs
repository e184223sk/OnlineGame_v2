using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TANK : MonoBehaviour
{
    Rigidbody rigidbody_;
    public float forwardSpeed;
     
    public ForceMode forceMODE;
    IsCaterpillarGrounding CL, CR;
    [System.NonSerialized]
    public Transform Turret, Cannon;
    public tank_Range CannonRange;
    public tank_weaponloader CannonLoding;

    public CaterpillarData caterpillar_L, caterpillar_R;
    public float TurretSpinSpeed;
    public float Torque;
    float tss, tsx;
    Vector3 mas;
    public float masSens;
    public float Stability;
    public float vect;
    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
        rigidbody_.drag = 4;
        rigidbody_.angularDrag = 0;
        Turret = transform.Find("Body/Turret");
        Cannon = transform.Find("Body/Turret/MainCannon"); 
        caterpillar_L.Init(transform.Find("Body/CaterpillarL"));
        caterpillar_R.Init(transform.Find("Body/CaterpillarR"));
        CL = transform.Find("Body/CaterpillarL").GetComponent<IsCaterpillarGrounding>();
        CR = transform.Find("Body/CaterpillarR").GetComponent<IsCaterpillarGrounding>();
        //rigidbody_.centerOfMass -= transform.up * 0.3f;Key.JoyStickL.Get.x
        mas = rigidbody_.centerOfMass;
        //rigidbody_.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (CL.IsGround && CR.IsGround)
        { 
            rigidbody_.MovePosition(rigidbody_.position + transform.forward * Key.JoyStickL.Get.y * forwardSpeed * Time.deltaTime); 
            rigidbody_.MoveRotation(rigidbody_.rotation *  Quaternion.Euler(0, Key.JoyStickL.Get.x * 360 * Torque * Time.deltaTime, 0));
            caterpillar_L.Update(Key.JoyStickL.Get.x * 3+ Key.JoyStickL.Get.y);
            caterpillar_R.Update(-Key.JoyStickL.Get.x * 3 + Key.JoyStickL.Get.y);
        }
        else if (!CL.IsGround && CR.IsGround)
        {
            rigidbody_.AddForceAtPosition(rigidbody_.position + transform.forward * Key.JoyStickL.Get.y * forwardSpeed * Time.deltaTime, caterpillar_R.root.position);
            rigidbody_.MoveRotation(rigidbody_.rotation * Quaternion.Euler(0, Key.JoyStickL.Get.x * 360 * Torque * Time.deltaTime, 0)); 
            caterpillar_R.Update(-Key.JoyStickL.Get.x * 3 + Key.JoyStickL.Get.y);
        }
        else if (CL.IsGround && !CR.IsGround)
        {
            rigidbody_.AddForceAtPosition(rigidbody_.position + transform.forward * Key.JoyStickL.Get.y * forwardSpeed * Time.deltaTime, caterpillar_L.root.position);
            rigidbody_.MoveRotation(rigidbody_.rotation * Quaternion.Euler(0, Key.JoyStickL.Get.x * 360 * Torque * Time.deltaTime, 0)); 
            caterpillar_R.Update(-Key.JoyStickL.Get.x * 3 + Key.JoyStickL.Get.y);
        }
        else
        {

        }

        Turret.Rotate(Vector3.up * Time.deltaTime * TurretSpinSpeed * Key.JoyStickR.Get.x * 360);

        if (tsx < 0) tsx = 0; else if (tsx > 1) tsx = 1;
        var vectNOW = Key.JoyStickR.Get.y; 
             Cannon.localRotation = Quaternion.Euler(CannonRange.min + Key.JoyStickR.Get.y  * (CannonRange.max - CannonRange.min), 0, 0);
        vect = Key.JoyStickR.Get.y ;

        if (Key.A.Down)
        {
            Debug.Log("fire");
        }

        if (Key.B.Down)
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
    public Transform root;
    public void Init(Transform root)
    {
        this.root = root;
        Mesh = root.GetComponent<MeshRenderer>();
        Wheels = new Transform[WheelCnt];
        for (int y = 0; y < WheelCnt; y++)
            Wheels[y] = root.Find("wheel"+y);
    }

    public void Update(float c)
    {
        foreach (var t in Wheels)
            t.Rotate(Vector3.up*Time.deltaTime*Radius* -c * SpeedSensiviry);
        Mesh.materials[0].mainTextureOffset += new Vector2( -c * SpeedSensiviry, 0);
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

//発射|サブ|se

 