using UnityEngine;

public class CameraCtrl : MonoBehaviour
{ 
    public float sensivity = 5, Distance;
    public Transform target;
    float x, y;
    Quaternion LastAngle;

    void Update()
    { 
        if (Input.GetKey(KeyCode.LeftArrow )) x -= Time.deltaTime * sensivity;
        if (Input.GetKey(KeyCode.RightArrow)) x += Time.deltaTime * sensivity;
        if (Input.GetKey(KeyCode.DownArrow )) y -= Time.deltaTime * sensivity;
        if (Input.GetKey(KeyCode.UpArrow   )) y += Time.deltaTime * sensivity;
        if (x > 90) x = 90; else if (x < -90) x = -90;
        if (y > 90) y = 90; else if (y < -90) y = -90;
        if (LastAngle != target.rotation)  x *= 0.95f;
        transform.rotation = Quaternion.Euler(0, x, 0) * target.rotation;
        transform.position = target.transform.position + target.transform.TransformDirection(Vector3.back) * Distance + Vector3.up * ((y+180)/90);
        LastAngle = target.rotation;
    }

}
