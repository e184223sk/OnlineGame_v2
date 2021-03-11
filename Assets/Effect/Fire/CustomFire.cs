using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFire : MonoBehaviour
{
    [Header("<本スクリプトはゲーム実行時に消えます>")]
    [Range(0.2f, 5f)]
    public float Radius = 2;
    [Range(0.1f, 1f)]
    public float Density = 1;
    [Range(0.1f, 3f)]
    public float DamageHeight;
    
    void Start()
    {
        SyncSubFire();
        Destroy(this);
    }



#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying) return;
        Gizmos.color = Color.cyan;

        float pd = Mathf.PI / 180;

        if (!UnityEditor.EditorApplication.isPlaying)
            SyncSubFire();

        if (Density < 0)
            Density = 0;
        else if (Density > 0.9)
            Density = 0.9f;

        Vector3 pp = new Vector3(transform.forward.z, 0, transform.right.x);
        for (float p = 0; p < Radius; p += (1- Density))
            for (int v = 0; v <360; v++)
                Gizmos.DrawLine
                (
                    transform.position + new Vector3(p * Mathf.Cos(v * pd) * pp.x, 0, p * Mathf.Sin(v * pd) * pp.z),
                    transform.position + new Vector3(p * Mathf.Cos((v + 1) * pd) * pp.x, 0, p * Mathf.Sin((v + 1) * pd * pp.z))
                );

    }
#endif



    void SyncSubFire()
    {
        ParticleSystem r = GetComponent<ParticleSystem>();
        ParticleSystem c = transform.Find("sub").GetComponent<ParticleSystem>();
        var re = r.emission;
        var ce = c.emission;
        var rs = r.shape;
        var cs = c.shape;
        re.rate   = ce.rate   = Radius * 100 * Density;
        rs.radius = cs.radius = Radius;
        transform.Find("root").position = transform.position - transform.Find("root").up * (DamageHeight / 2 - 0.2f);
        Transform[] collider = new Transform[10];
       
        for (int v = 0; v < 10; v++)
        {
            collider[v] = transform.Find("root/col" + v);
            collider[v].localRotation = Quaternion.Euler(0, v * 18, 0);
            collider[v].localScale = new Vector3(Radius * Mathf.PI / 10, DamageHeight, Radius * 2);
        }
    }


}


