using UnityEngine;

public class HoleDirectionSetter : MonoBehaviour
{
    bool c;
    public static Camera camera;
    void Update()
    {
        if (camera != null) transform.LookAt(camera.transform.position);
        RaycastHit hit;

        if (!c && Physics.Raycast(transform.position + transform.forward * 0.1f, -transform.forward, out hit, 0.4f))
                if (hit.collider.gameObject != gameObject)
                {
                    transform.position = hit.point;
                    c = true;
                }


    }
}
