using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

 
public class ReflectionProbeTag : MonoBehaviour
{
    public uint TAG;
}


#if UNITY_EDITOR
public class ReflectionProbeTagManager
{
    [InitializeOnLoadMethod]
    static void init() { EditorApplication.update += UPDATE; }
    static uint sd;
    static void UPDATE()
    {
        if (EditorApplication.isPlaying) return;
        sd = 0;
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理.
            if (o.activeInHierarchy)
            {
                if (o.GetComponent<NotReflectionProbeTag>() != null)
                { 
                    var co = o.GetComponent<ReflectionProbe>();

                    if (co != null)
                    {
                        if(o.GetComponent<ReflectionProbeTag>()==null)
                            o.AddComponent<ReflectionProbeTag>();
                        co.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;

                        o.GetComponent<ReflectionProbeTag>().TAG = sd;
                        sd += 1;
                    }
                }
            }
        } 
    }
}

#endif


