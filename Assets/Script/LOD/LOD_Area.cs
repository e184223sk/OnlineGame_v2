using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_Area : MonoBehaviour
{
    public bool IsActive;
    public bool LastActive;
    public float size;
    public LOD_obj[] lods;

    public const float _HIGHAREA =45;
    public const float _MIDAREA = 100;
    public const float _LOWAREA = 150; 

    void Start()
    {
        int h = 0;
        List<LOD_obj> lod = new List<LOD_obj>();
        foreach (Transform obj in this.gameObject.transform)
        {
            if (obj.gameObject.GetComponent<LOD>() != null)
            {
                var t = obj.gameObject.GetComponent<LOD>();
                if (t.transform.Find("High") == null) Debug.Log(t.transform.name + "/:" + h + "::High");
                if (t.transform.Find("Mid" ) == null) Debug.Log(t.transform.name + "/:" + h + "::Mid");
                if (t.transform.Find("Low" ) == null) Debug.Log(t.transform.name + "/:" + h + "::Low");
                lod.Add(new LOD_obj()
                {
                    highObj = t.transform.Find("High").gameObject,
                    midObj  = t.transform.Find("Mid").gameObject,
                    lowObj  = t.transform.Find("Low").gameObject,
                    root    = t.transform
                });
            }
            h++;
        }
        lods = lod.ToArray();
        LastActive = true;
    }


    void Update()
    {
        if (IsActive)
        {
            Debug.Log("GG");
            float dis;
            Vector3 point = LOD_Root.target.transform.position;
            foreach (var a in lods)
            {
                dis = Vector3.Distance(a.root.position, point);
                Debug.Log("gr:" + dis);
                a.highObj.active = dis < _HIGHAREA;
                a.midObj.active = dis < _MIDAREA & dis > _HIGHAREA;
                a.lowObj.active = dis < _LOWAREA & dis > _MIDAREA;
                Debug.Log(a.highObj.active ? "H" : (a.midObj.active ? "M" : (a.lowObj.active ? "L" : "X")));
            }
        }
        else if (LastActive)
        {
            foreach (var a in lods)
            {
                a.highObj.gameObject.active = 
                a.midObj.gameObject.active  = 
                a.lowObj.gameObject.active  = 
                false;
            }
        }

        LastActive = IsActive;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsActive ? Color.cyan : Color.red; 
        Gizmos.DrawWireSphere(transform.position, size); 
        foreach (var a in lods)
        {
            Gizmos.color = a.highObj.gameObject.active ? Color.yellow : Color.gray;
            Gizmos.DrawWireSphere(a.root.transform.position, _HIGHAREA);
            Gizmos.color = a.midObj.gameObject.active ? Color.cyan: Color.gray;
            Gizmos.DrawWireSphere(a.root.transform.position, _MIDAREA);
            Gizmos.color = a.lowObj.gameObject.active ? Color.green: Color.gray;
            Gizmos.DrawWireSphere(a.root.transform.position, _LOWAREA);
        }
    }
}


[System.Serializable]
public class LOD_obj
{
    public GameObject highObj, midObj, lowObj;
    public Transform root;
}