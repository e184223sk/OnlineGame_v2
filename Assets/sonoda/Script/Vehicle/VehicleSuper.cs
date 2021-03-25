using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class VehicleSuper : MonoBehaviour
{
    //
    #region Getter
    //スピードのゲッター変数
    public float _getSpeed { get { return _speed; } }

    //最高速度のゲッター変数
    public int _getMaxSpeed { get { return _MaxSpeed; } }

    //バックの最高速度のゲッター変数
    public int _getMinSpeed { get { return _MinSpeed; } }


    #endregion

    #region Vehicle Properties

    //リジッドボディ
    private Rigidbody _rigidbody;


    //加速に加える力
    [SerializeField]
    private float _AccelPower = 5;

    //減速に加える力
    [SerializeField]
    private float _DecelPower = 5;


    //回転に加える力
    [SerializeField]
    private float _TurnPower = 5;



    //乗り物の速度
    [SerializeField]
    protected float _speed;


    //乗り物の最大速度
    [Range(0, 150), SerializeField]
    protected int _MaxSpeed;

    //乗り物のバックの最高速度
    [Range(-50, 0), SerializeField]
    protected int _MinSpeed;

    [SerializeField]
    protected Vector3 _CenterOfMass;

    //完全静止状態のvelocity.magnitudeの閾値  
    [SerializeField, Range(1, 5)]
    private float _stopThre = 5;

    #endregion

    #region Wheel Properties

    //タイヤのGameObject f = 前輪　b = 後輪　r = 右側 l = 左側    2輪の場合は_wheel_frと _wheel_br を使う
    private WheelInfo _wheel_fr, _wheel_fl, _wheel_br, _wheel_bl;

    //タイヤ情報の配列
    private WheelInfo[] _wheels;

    private string[] _WheelNames = new string[]
    {
        "wheel_fr",
        "wheel_fl",
        "wheel_br",
        "wheel_bl",
    };

    //4輪かどうか
    [SerializeField]
    protected bool _Is4Wheels = true;

    //前輪の最大回転角
    [SerializeField, Range(0, 90)]
    protected float _MaxSteeringAngle;

    //後輪の最大回転速度
    [SerializeField]
    protected float _MaxMotorTorque;

    #endregion


    #region Initialiezer

    protected void Initialized()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _CenterOfMass;


        //mass以外は固定

        //タイヤ取得   4輪------------------------------------
        if (_Is4Wheels)
        {
            _wheels = new WheelInfo[4];
            for (int i = 0; i < 4; i++)
            {
                _wheels[i] = new WheelInfo(_WheelNames[i], i < 2, i % 2 == 0);
            }
        }
        else  // 2輪
        {
            _wheels = new WheelInfo[2];
            _wheels[0] = new WheelInfo(_WheelNames[0], true, false);
            _wheels[1] = new WheelInfo(_WheelNames[2], false, false);
        }

        foreach (var w in _wheels)
        {
            w._collider.forceAppPointDistance = _CenterOfMass.y - 0.5f;
        }

    }
    #endregion

    #region Driving Methods

    //子クラスで呼ぶ運転メソッド
    protected void Drive()
    {


        // _rigidbody.velocity *= 0.99f;

        //前進後退
        MoveFB();


        //左右に曲がる
        // TurnLR();


        //タイヤ制御と左右回転
        WheelCotroll();

        //デバッグ用に出力
        _speed = _rigidbody.velocity.magnitude;

        // Debug.Log(_rigidbody.velocity);
    }

    //前進
    private void MoveFB()
    {
        Vector2 vertical = Key.JoyStickL.Get;

        //入力がないときに減速処理
        /* if (vertical.magnitude == 0)
         {
             if (_rigidbody.velocity.magnitude < _stopThre) return;
             _rigidbody.AddRelativeForce(_rigidbody.velocity.z > 0 ? -Vector3.forward : Vector3.forward* _DecelPower * Time.deltaTime, ForceMode.Acceleration);
         }
        */
        //最高速を上回ったら加速しない        ||      最低速を下回ったら減速しない
        if (_rigidbody.velocity.magnitude > _MaxSpeed && vertical.y > 0)
            return;
        if (_rigidbody.velocity.magnitude > _MinSpeed && _rigidbody.velocity.z < 0 && vertical.y < 0)
            return;
      

/*        if (_speed > _MaxSpeed && vertical.y > 0)
            return;
        if (_speed < _MinSpeed && vertical.y < 0)
            return;
*/
        _speed += vertical.y > 0 ? _AccelPower : -_AccelPower;

        //_rigidbody.MovePosition(_rigidbody.position + transform.forward * _speed * Key.JoyStickL.Get.y * Time.deltaTime);
        _rigidbody.AddRelativeForce(Vector3.forward * vertical.y * _AccelPower * Time.deltaTime, ForceMode.Force);
    }



    private void TurnLR()
    {
        if (_rigidbody.velocity.magnitude < _stopThre) return;

        Vector2 horizontal = Key.JoyStickL.Get;

        _rigidbody.AddRelativeTorque(Vector3.up * horizontal.x * _TurnPower * Time.deltaTime, ForceMode.VelocityChange);



        //_rigidbody.AddRelativeForce(Vector3.forward * -horizontal.y * (_AccelPower / 2) * Time.deltaTime, ForceMode.Acceleration);
    }

    private void WheelCotroll()
    {

        float horizontal = Key.JoyStickL.Get.x;
        float vetrical = Key.JoyStickL.Get.y;
        float steering = _MaxSteeringAngle * horizontal;
        float motor = _MaxMotorTorque * vetrical * _AccelPower;

        foreach (var w in _wheels)
        {

            /* Vector3 position;
             Quaternion rotation;
             w._collider.GetWorldPose(out position, out rotation);
             w._object.transform.rotation = rotation;

             */
            if (horizontal == 0f)
            {
                w._collider.steerAngle = 0;
            }
            if (w._isFront)
            {


                //右に曲がるときは右前輪の方の回転数を減らす
                if (horizontal > 0)
                    w._collider.steerAngle = w._isRight ? steering * 0.8f : steering;
                //左に曲がるときは左前輪の方の回転数を減らす
                else
                    w._collider.steerAngle = w._isRight ? steering : steering * 0.8f;

            }
            else w._collider.motorTorque = motor;

        }
    }
    /*
     //wheelcolliderの回転速度に合わせてタイヤモデルを回転させる
        wheelFLTrans.Rotate( wheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelFRTrans.Rotate( wheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelBLTrans.Rotate( wheelBL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelBRTrans.Rotate( wheelBR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        
        //wheelcolliderの角度に合わせてタイヤモデルを回転する（フロントのみ）
        wheelFLTrans.localEulerAngles = new Vector3(wheelFLTrans.localEulerAngles.x, wheelFL.steerAngle - wheelFLTrans.localEulerAngles.z, wheelFLTrans.localEulerAngles.z);
        wheelFRTrans.localEulerAngles = new Vector3(wheelFRTrans.localEulerAngles.x, wheelFR.steerAngle - wheelFRTrans.localEulerAngles.z, wheelFRTrans.localEulerAngles.z);*/


    #endregion


    #region Unity Callbacks

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_CenterOfMass, 0.5f);

        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(_emphasis, 0.3f);
    }



    #endregion
}
