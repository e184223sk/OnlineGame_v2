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
        if(cam == null)
            Debug.LogError("LOD対象のカメラがセットされていません");
    }
    
    
    void FixedUpdate()
    {
        if (cam != null)
            foreach (var a in area) 
                a.IsActive = Vector3.Distance(a.transform.position, cam.transform.position) < a.size;
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
