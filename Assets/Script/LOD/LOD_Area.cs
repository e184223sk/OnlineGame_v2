using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_Area : MonoBehaviour
{
    public bool IsActive;
    bool LastActive;
    public float size;
    public LOD_obj[] lods;

    public const float _HIGHAREA = 10;
    public const float _MIDAREA = 30;
    public const float _LOWAREA = 50; 

    void Start()
    { 
        List<LOD_obj> lod = new List<LOD_obj>();
        foreach (GameObject obj in gameObject.transform)
            if (obj.GetComponent<LOD>() != null)
            {
                var t = obj.GetComponent<LOD>();
                lod.Add(new LOD_obj()
                {
                    highObj = t.transform.Find("High").gameObject,
                    midObj  = t.transform.Find("Mid").gameObject,
                    lowObj  = t.transform.Find("Low").gameObject,
                    root    = t.transform
                });

            }
        lods = lod.ToArray();
    }


    void Update()
    {
        if (LastActive != IsActive)
        {
            float dis;
            Vector3 point = LOD_Root.target.transform.position;
            if (IsActive)
                foreach (var a in lods)
                {
                    dis = Vector3.Distance(a.root.position, point);
                    a.highObj.active = dis < _HIGHAREA;
                    a.midObj .active = dis < _MIDAREA & dis > _HIGHAREA;
                    a.lowObj .active = dis < _LOWAREA & dis > _MIDAREA; 
                }
            else
                foreach (var a in lods)
                    a.highObj.gameObject.active =
                    a.midObj.gameObject.active =
                    a.lowObj.gameObject.active =
                    false; 

            LastActive = IsActive;
        }
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = IsActive ? Color.cyan : Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 100, new Vector3(size, 200, size));
    }
}


[System.Serializable]
public class LOD_obj
{
    public GameObject highObj, midObj, lowObj;
    public Transform root;
}