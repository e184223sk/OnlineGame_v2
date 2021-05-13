using UnityEngine;

public class targetPointer : MonoBehaviour
{
    public WeaponSelector selector;
    public float near,far, distance, upper, scaleAdj, DrawUpper;
    public Transform cameraTransform;
    public Transform player;
    MeshRenderer render;
    [System.NonSerialized]
    public Vector3 targetingPOINT;
    public void Start()
    { 
        
        selector = player.transform.GetComponent<WeaponSelector>();
        render = GetComponent<MeshRenderer>();
    }

    

    void Update()
    {


        if(player == null)
        {
            player = GameObject.Find("Player").transform;
            Start();
        }

        transform.position = cameraTransform.position;
        transform.rotation = cameraTransform.rotation;

        RaycastHit h,h2;
        Ray ray = new Ray(transform.position + transform.up * upper + transform.forward * near, transform.forward);
        Vector3 BoxCastScale = transform.localScale + Vector3.forward * transform.localScale.x;

        float dis = distance;
        //if (Physics.BoxCast(ray.origin, BoxCastScale, ray.direction, out h, Quaternion.identity, distance))
        //  if(h.transform.root.gameObject != selector.transform.gameObject)
        //dis = Vector3.Distance(h.point, cameraTransform.position);

        transform.position = ray.origin + ray.direction * dis + cameraTransform.up * DrawUpper + transform.forward * -far;
        transform.localScale = Vector3.Distance(cameraTransform.position, transform.position) *  scaleAdj * new Vector3(1, 1, 0.01f);  


        //色変換/表示非表示/DebugRayLine----------------------------
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 0.04f, false);
        render.enabled = selector.GetWeapon.GetComponent<GunBehavior>() != null;
        render.material.SetColor("_EmissionColor", Color.cyan);

        if (Physics.Raycast(ray.origin,  ray.direction, out h2))
        {
            targetingPOINT = h2.point;
            if 
            (
                h2.transform.root.gameObject != selector.transform.gameObject &&
                h2.transform.root.GetComponent<BulletBehaviour>() != null ||
                h2.transform.root.GetComponent<PLAYERS>() != null
            )
                render.material.SetColor("_EmissionColor", Color.red);
        }
    }

}
