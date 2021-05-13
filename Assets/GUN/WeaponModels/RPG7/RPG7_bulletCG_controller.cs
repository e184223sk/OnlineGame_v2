using UnityEngine;

public class RPG7_bulletCG_controller : MonoBehaviour
{
    GunController gcon;
    GameObject BulletCG;

    void Start()
    {
        BulletCG = transform.Find("BulletCG").gameObject;
        gcon = GetComponent<GunController>();
    }
     
    void Update() => BulletCG.active = gcon.Loaded.now != 0; 
}
