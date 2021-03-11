using UnityEngine;

public class shipmove : MonoBehaviour
{
    public float speed = 30;
    private float AngleY;
    public float posy;
    public float turnspeed;

    // Update is called once per frame
    void Update()
    {
        //操作
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(speed * Vector3.forward * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(speed * Vector3.back * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            AngleY -= turnspeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            AngleY += turnspeed * Time.deltaTime;
        }
        //位置
        transform.position = new Vector3(transform.position.x, Wave.GetSurfaceHeight(transform.position) + posy, transform.position.z);

        //角度
        Quaternion rot = Wave.GetSurfaceNormal(transform.position);
        rot = new Quaternion(rot.x, AngleY, rot.z, rot.w);
        transform.rotation = rot;

    }
}
