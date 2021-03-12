using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testship : MonoBehaviour
{
    float _angleH;
    public float p;
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
         
        if (Input.GetKeyDown(KeyCode.D))
        {
            _angleH += 10 * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _angleH -= 10 * Time.deltaTime;
        }

        transform.position = new Vector3(transform.position.x, Wave.GetSurfaceHeight(transform.position, true)*p, transform.position.z);
        transform.rotation = Wave.GetSurfaceNormal(transform.position);
        /*
         
        Quaternion a = Wave.GetSurfaceNormal(transform.position);
        a = new Quaternion(a.x, _angleH, a.z , a.w);
      //  transform.rotation = Quaternion.Euler(a.x * p, _angleH,a.z * -p);
         */
    }
}
