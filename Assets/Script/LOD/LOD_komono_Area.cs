using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_komono_Area : MonoBehaviour
{
    public bool IsActive;
    public bool LastActive;
    public float size;
    public Transform[] lods;

    public const float AREA = 11; 

    void Start()
    { 
        List<Transform> lod = new List<Transform>();
        foreach (Transform obj in this.gameObject.transform)
        {
            if (obj.gameObject.GetComponent<LOD_komono>() != null)
            {
                lod.Add(obj.Find("obj"));
                Destroy(obj.gameObject.GetComponent<LOD_komono>()); 
            } 
        }
        lods = lod.ToArray();
        LastActive = true;
    }


    void Update()
    {
        if (IsActive)
        {
            foreach (var a in lods)
                a.gameObject.active = Vector3.Distance(a.position, LOD_komono_Root.target.transform.position) < AREA;  
        }
        else if (LastActive)
        {
            foreach (var a in lods)
                a.gameObject.active = false; 
        }

        LastActive = IsActive;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsActive ? Color.cyan : Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
        foreach (var a in lods)
        {
            Gizmos.color = a.gameObject.active ? Color.magenta : Color.gray;
            Gizmos.DrawWireSphere(a.root.transform.position, AREA); 
        }
    }
}


