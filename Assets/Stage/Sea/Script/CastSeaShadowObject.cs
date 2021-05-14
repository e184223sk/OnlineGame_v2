using System.Collections.Generic;
using UnityEngine;

public class CastSeaShadowObject : MonoBehaviour
{  
    void Staxrt()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            List<Material> uu = new List<Material>(mr.sharedMaterials);
            foreach (var r in Wave.wave.SeaShadow) uu.Add(r.material);
            mr.sharedMaterials = uu.ToArray();
             
            if (Wave.wave.CastSeaShadowBaseList == null)
            {
                Wave.wave.CastSeaShadowBaseList = new System.Collections.Generic.List<MeshRenderer>();
            }
            Wave.wave.CastSeaShadowBaseList.Add(mr);
            Wave.wave.CastSeaShadowBaseObj = Wave.wave.CastSeaShadowBaseList[0];

        }
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        DestroyImmediate(this);
    }
}

