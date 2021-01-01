using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAvatorScene_ctrl : MonoBehaviour
{
    //CG CAMERA -------------------------------------------------
    [Space(20)]
    [SerializeField] Transform _CameraRoot;
    [SerializeField] Transform _Camera;
    [SerializeField] Transform _ModelRoot;
    [SerializeField] float VerticalMove;
    [SerializeField] float Radius;
    [SerializeField] float RadiusMin;
    [SerializeField] float SpinSpeed;
    float v;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CG CAMERA ------------------------------------------------
        v += Time.deltaTime;
        _CameraRoot.position = _ModelRoot.position + Vector3.up * Mathf.Sin(v);
        _Camera.position = _CameraRoot.position;
        _Camera.LookAt(_CameraRoot);
        _Camera.Translate(Vector3.back *(RadiusMin + Mathf.Sin(v) * Radius),Space.Self);
        _CameraRoot.Rotate(Vector3.up * SpinSpeed * Time.deltaTime, Space.World);
        //UI System------------------------------------------------------

        //UserModel Update ---------------------------------------------------
    }
}
