using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCtrler : VehicleSuper
{
    
    //各種パラメータの設定
    void Start()
    {
        Initialized();
        //_accel = 10;
        _MaxSpeed = 120;
        _MinSpeed = 30;
        _speed = 0;
        /*_steering = 30;
        _slippery = 50;
        _inertia = 3f;
        _fliction = 0.7f;*/
    }

    // Update is called once per frame
    void Update()
    {
       Drive();
    }
}
