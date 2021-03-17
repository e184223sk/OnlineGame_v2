using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class VehicleSuper : MonoBehaviour
{
    //
    #region Public Properties
    //スピードのゲッター変数
    public float _getSpeed { get { return _speed; } }

    //最高速度のゲッター変数
    public int _getMaxSpeed { get { return _MaxSpeed; } }

    //バックの最高速度のゲッター変数
    public int _getMinSpeed { get { return _MinSpeed; } }


    #endregion

    #region Private Variables

    //リジッドボディ
    private Rigidbody _rigidbody;


    //加速に加える力
    [SerializeField]
    private float _AccelPower = 5;

    //加速に加える力
    [SerializeField]
    private float _DecelPower = 5;


    //回転に加える力
    [SerializeField]
    private float _TurnPower = 5;


    //完全静止状態のvelocity.magnitudeの閾値  要るのか？
    [SerializeField,Range(1,5)]
    private float _stopThre = 5;

    [Lang_Jp("力点"),SerializeField]
    //回転の時、力を加えるポイント
    private Vector3 _emphasis;

    //
    private Collider _collider;

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

    #endregion

    #region Protected Variables

    //乗り物の速度
    [SerializeField]
    protected float _speed;


    //乗り物の最大速度
    [Range(0,150),SerializeField]
    protected int _MaxSpeed;

    //乗り物のバックの最高速度
    [Range(-50,0),SerializeField]
    protected int _MinSpeed;

    [SerializeField]
    protected Vector3 _CenterOfMass;

    //4輪かどうか
    [SerializeField]
    protected bool _Is4Wheels = true;

    //前輪の最大回転角
    [SerializeField , Range(0,90)]
    protected float _MaxSteeringAngle;

    //後輪の最大回転速度
    [SerializeField]
    protected float _MaxMotorTorque;
    #endregion


    #region Protected Methods

    protected void Initialized()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _CenterOfMass;
        //mass以外は固定

        //タイヤ取得   4輪------------------------------------
        if (_Is4Wheels)
        {
            _wheels = new WheelInfo[4];
            for(int i = 0; i < 4; i++){
                _wheels[i] = new WheelInfo(_WheelNames[i] , i < 2);
            }
        }
        else  // 2輪
        {
            _wheels = new WheelInfo[2];
            _wheels[0] = new WheelInfo(_WheelNames[0], true);
            _wheels[1] = new WheelInfo(_WheelNames[2], false);
        }

    }

    protected void Drive()
    {

        //前進後退
        MoveFB();


        //左右に曲がる
       // TurnLR();

        //タイヤ制御と左右回転
        WheelCotroll();

        _speed = _rigidbody.velocity.magnitude;
        
    }

    #endregion

    #region Private Methods
    //前進
    private void MoveFB()
    {
        Vector2 vertical = Key.JoyStickL.Get;

        //入力がないときに減速処理
        if (vertical.magnitude == 0)
        {
            _rigidbody.AddRelativeForce(new Vector3(0, 0, -vertical.y) * _DecelPower * Time.deltaTime, ForceMode.Acceleration);
        }

        //最高速を上回ったら加速しない        ||      最低速を下回ったら減速しない
        if (_rigidbody.velocity.magnitude > _MaxSpeed && vertical.y > 0 || _rigidbody.velocity.magnitude < _MinSpeed && vertical.y < 0)
            return;

        _rigidbody.AddRelativeForce(new Vector3(0, 0, vertical.y) * _AccelPower * Time.deltaTime, ForceMode.Acceleration);
        
        /*
        //最高速度に対して今どのくらいの速度なのかの割合
        _accelCurveRate = _speed / _MaxSpeed;

        //カーブの位置に応じて加速度を変更
        _accel = _accelCurve.Evaluate( _accelCurveRate )* 25;
        _speed += _accel * Time.deltaTime;

        //最大速度を超えないように
        if (_speed > _MaxSpeed)
            _speed = (float)_MaxSpeed;
        */
    }



    private void TurnLR()
    {
        if (_rigidbody.velocity.magnitude < _stopThre) return;

        Vector2 horizontal = Key.JoyStickL.Get;


        //_rigidbody.AddForce(-Vector3.up * 500* Time.deltaTime,ForceMode.Acceleration);
        //_rigidbody.AddRelativeTorque(Vector3.up * horizontal.x * _TurnPower * Time.deltaTime, ForceMode.Acceleration);

        _rigidbody.AddForceAtPosition(new Vector3( horizontal.x , 0f,0f)* Time.deltaTime * _TurnPower,transform.InverseTransformDirection( _emphasis),  ForceMode.Acceleration);
    }


    private void WheelCotroll()
    {
        float steering = _MaxSteeringAngle * Key.JoyStickL.Get.x;
        float motor = _MaxMotorTorque + Key.JoyStickL.Get.y;

        foreach(var w in _wheels)
        {
            Vector3 position;
            Quaternion rotation;
            w._collider.GetWorldPose(out position, out rotation);
            w._object.transform.rotation = rotation;

            if (w._isFront)     w._collider.steerAngle = steering;
            else                w._collider.motorTorque = motor;
        }
    }


    #endregion


    #region Unity Callbacks

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_CenterOfMass, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_emphasis, 0.3f);
    }



    #endregion
}
