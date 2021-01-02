using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : DynamicObjectBehavior
{

    [Lang_Jp("プロペラ軸のオブジェクト")]
    public Transform MainPropellerShaft;
    [Lang_Jp("テイルローター軸のオブジェクト")]
    public Transform TailPropellerShaft;

    [Space(30)]
    [Lang_Jp("浮力")]
    public float Buoyancy;
    [Lang_Jp("回転力")]
    public float SpinPower;

    [Lang_Jp("機首上げ操作の感度")]
    public float c;


    [Lang_Jp("プロペラの加速度")]
    public float MainPropellerAcceleration;

    [Lang_Jp("テイルローターの加速度")]
    public float TailPropellerAcceleration;

    [Lang_Jp("減速度")]
    [Range(0, 1)]
    public float DecelerationRate = 0.9f;


    [Lang_Jp("水平安定率")]
    public float HorizontalRecoveryRate;

    float MainPropellerSpeed, TailPropellerSpeed;
    Rigidbody rigit;
    float xc;
    
    void Start()
    {
        rigit = GetComponent<Rigidbody>();
    }
     
    void Update()
    {
        if (Key.A.Press) xc += Time.deltaTime * c;
        if (Key.B.Press) xc -= Time.deltaTime * c;
        xc = MinCut(xc*DecelerationRate);

        var input = (Vector2)Key.JoyStickL * Time.deltaTime;

        MainPropellerSpeed = MinCut((MainPropellerSpeed + (input.y * MainPropellerAcceleration)) * DecelerationRate);
        TailPropellerSpeed = MinCut((TailPropellerSpeed + (input.x * TailPropellerAcceleration)) * DecelerationRate);
        MainPropellerShaft.Rotate(MainPropellerSpeed * Vector3.up, Space.Self);
        TailPropellerShaft.Rotate(TailPropellerSpeed * Vector3.up, Space.Self);
        rigit.AddRelativeForce (Vector3.up * MainPropellerSpeed * Buoyancy , ForceMode.Acceleration); //浮力
        rigit.AddRelativeTorque(Vector3.up * TailPropellerSpeed * SpinPower, ForceMode.Acceleration); //
        rigit.AddRelativeTorque(Vector3.left * xc , ForceMode.Acceleration); //
        rigit.AddRelativeTorque(-Vector3.Scale(transform.rotation.ToEuler(), new Vector3(1,0,1)) * HorizontalRecoveryRate * Time.deltaTime, ForceMode.Acceleration); //
    }

    float MinCut(float raw) { return -0.1f < raw && raw < 0.1f ? 0 : raw; }
}
