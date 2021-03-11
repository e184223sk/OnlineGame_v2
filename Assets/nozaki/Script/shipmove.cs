using UnityEngine;

public class shipmove : MonoBehaviour
{
    public float speed = 1;
    public float torque = 5;
    public float heightAdjusted = 15;
    private float AngleY;
    public float addspeed = 5;

    // Update is called once per frame
    void Update()
    {
        Debug.LogError("dd");
        //操作
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(speed * Vector3.forward * Time.deltaTime, Space.Self) ;
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(speed * Vector3.back *Time.deltaTime, Space.Self);
        if (Input.GetKey(KeyCode.LeftArrow)) AngleY -= torque * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) AngleY += torque * Time.deltaTime;

        //加速
        if (Input.GetKey(KeyCode.Space)) speed += addspeed;

        //位置
        transform.position = new Vector3(transform.position.x, Wave.GetSurfaceHeight(transform.position) + heightAdjusted, transform.position.z);

        //角度
        transform.rotation = Wave.GetSurfaceNormal(transform.position) * Quaternion.Euler(0, AngleY, 0);
    }
}
