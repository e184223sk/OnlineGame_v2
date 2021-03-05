using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneCtrl : MonoBehaviour
{
    Rigidbody rigidbody_;
    [Range(0, 200)]
    public float Accel;
    [Range(0, 1)]
    public float Buoyancy;
    public float PostureRetention;
    public Vector2 power;
    //public Vector3 massCenter;fff
    public bool IsGround;

    public Vector3 GroundCheckPoint;
    public float CheckDistance;

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }
     
    void FixedUpdate()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        IsGround = false;
        if (Physics.Raycast(transform.position + GroundCheckPoint, -transform.up, out hit, CheckDistance))
            if (transform.root != hit.transform.root)
                IsGround = true;


        if (IsGround)
        {
            var v = transform.rotation.x;
            PostureRetention = Accel/((180 - Mathf.Abs(v)) / 10);
            
            float p = v > -90 && v < 0 ? (v + 90) / 90 : ((v < 90 && v > 0) ? (v - 90) / 90 : -1); //姿勢補間力ff
            rigidbody_.AddRelativeForce
            (
                new Vector3()
                {
                    x = 0,
                    y = Accel * Time.deltaTime * Buoyancy * (v > -90 && v < 90 ? -v / 90 : 0),
                    z = Accel * Accel * Time.deltaTime,
                },
                ForceMode.Force
            );
            rigidbody_.AddRelativeTorque(0, power.x * Time.deltaTime, 0, ForceMode.Acceleration);
            rigidbody_.AddRelativeTorque((Accel * -power.y + p * PostureRetention) * Time.deltaTime, 0, 0, ForceMode.Force); 
        }
        else
        {
            PostureRetention = 200 - Accel;
            var v = transform.rotation.x;
            float p = v > -90 && v < 0 ? (v + 90) / 90 : ((v < 90 && v > 0) ? (v - 90) / 90 : -1); //姿勢補間力ff
            rigidbody_.AddRelativeForce
            (
                new Vector3()
                {
                    x = 0,
                    y = Accel * Time.deltaTime * Buoyancy * (v > -90 && v < 90 ? -v / 90 : 0),
                    z = Accel * Accel * Time.deltaTime,
                },
                ForceMode.Force
            );
            rigidbody_.AddRelativeTorque(0, power.x * Time.deltaTime, 0, ForceMode.Acceleration);
            rigidbody_.AddRelativeTorque((Accel * -power.y + p * PostureRetention) * Time.deltaTime, 0, 0, ForceMode.Force); 
        }

    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + GroundCheckPoint, 0.1f);
        Gizmos.DrawLine(transform.position + GroundCheckPoint, GroundCheckPoint - Vector3.down * CheckDistance);
    }


}


/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneCtrl : MonoBehaviour
{
    Rigidbody rigidbody_;
    public float Accel, Buoyancy;

    public float spin;

    public float a;

    [Range(-1,1)]
    public float b;
    public float c;


    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKey(KeyCode.T)) Buoyancy += Time.deltaTime * b;
        if (Input.GetKey(KeyCode.G)) Buoyancy -= Time.deltaTime * b;

        Accel += input.y * Time.deltaTime *a;
        if (-0.03 < Accel && Accel < 0.03)
            Accel = 0;
        if (Accel < -1000) Accel = -1000;
        if (Accel > 3000) Accel = 3000;
        Accel *= 0.97f;
        Buoyancy *= 0.97f;

        // 
        rigidbody_.AddRelativeTorque
        (
           Time.deltaTime * new Vector3
            (
                -Buoyancy * (Accel < 0 ? 0 : Accel) * c * (90 - Mathf.Abs(-transform.rotation.eulerAngles.x)) / 90,
                input.y * spin,
                0
            )
            , ForceMode.Impulse
        );
        ///  float xxx = -transform.rotation.eulerAngles.x; 
        /// Buoyancy += Time.deltaTime * (Mathf.Abs(xxx) > 45 ? xxx%45 : xxx); 
        rigidbody_.AddRelativeForce(new Vector3(0, 0, 1)* Accel * Time.deltaTime, ForceMode.Acceleration);
        rigidbody_.AddRelativeTorque(-Vector3.Scale(transform.rotation.ToEuler(), new Vector3(1, 0, 1)) * Accel * Time.deltaTime, ForceMode.Acceleration); //

    }
}

     
     
     */
