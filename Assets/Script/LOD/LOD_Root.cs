using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_Root : MonoBehaviour
{
    public Camera cam;
    public LOD_Area[] area;
    Vector3 lastPoint;

    [Range(0.1f, 20f)]
    public float MovementThreshold;

    public static Camera target { get => me.cam; set => me.cam = value; }
    public static float movementThreshold { get => me.MovementThreshold; set => me.MovementThreshold = value > 20 ? 20 : (value < 0 ? 0: value); }
    static LOD_Root me;

    void Start()
    {
        me = this;
        List<LOD_Area> lla = new List<LOD_Area>();
        foreach (var obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            foreach (var a in obj.GetComponentsInChildren<LOD_Area>(true))
                lla.Add(a);
        area = lla.ToArray();
    }
    
    
    void Update()
    {
        if (cam == null) return;

        if (Vector3.Distance(lastPoint, transform.position) > MovementThreshold)
        { 
            foreach (var a in area)
            {
                Vector3 x = a.transform.position - cam.transform.position;
                a.IsActive = (x.x * x.x) + (x.z * x.z) < a.size * a.size + a.size * a.size;
            }
            lastPoint = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (cam != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cam.transform.position, 0.4f);
        }
    }
}
