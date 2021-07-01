using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLOD_Trees : MonoBehaviour
{
    [ContextMenu("Method")]
    private void Method()
    {
        var H = transform.Find("High");
        var M = transform.Find("Mid");
        var L = transform.Find("Low");
        if (H.GetComponent<LOD_komono>() == null) H.gameObject.AddComponent<LOD_komono>();
        if (M.GetComponent<LODTREE_DELETER>() == null) M.gameObject.AddComponent<LODTREE_DELETER>();
        if (L.GetComponent<LODTREE_DELETER>() == null) L.gameObject.AddComponent<LODTREE_DELETER>();
    }
}
