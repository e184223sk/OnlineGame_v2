using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneCtrl : MonoBehaviour
{
    Rigidbody rigidbody_;
    public float Accel, Spin;
    [Range(0,1)]
    public float Buoyancy;
    public Vector3 massCenter;

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }
     
    void FixedUpdate()
    { 
        rigidbody_.AddRelativeForce(0, Accel * Accel * Time.deltaTime * Buoyancy, Accel * Time.deltaTime, ForceMode.Acceleration);
        rigidbody_.AddRelativeTorque(Accel * Accel * Time.deltaTime * Buoyancy, Spin, 0, ForceMode.Acceleration);
        rigidbody_.centerOfMass = Vector3.Scale(transform.forward, massCenter);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.Scale(transform.forward, massCenter) + transform.position, 1.3f);
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
