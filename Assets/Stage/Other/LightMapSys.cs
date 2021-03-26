using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class LightMapSys : MonoBehaviour
{
    public bool UseMap;

    [SerializeField] ReflectionProbe probeComponent;

    void Start()
    {
    }

    // Update is called once per frame
    void Update() => _UPDATE();

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
            _UPDATE();
    }
#endif

    void _UPDATE()
    {
        if (UseMap)
        {

        }
        else
        {

        }
    }

    void SetLightmap(LightmapData[] lmds, LightProbes lp, Texture t)
    {
        string x = "LightMap/" + 
            TimeData.clock.hour.ToString().PadLeft(2, '0') + "h" + 
            TimeData.clock.min.ToString().PadLeft(2, '0')+"m/";

        LightmapSettings.lightmaps = lmds;
       // LightmapSettings.lightProbes = Resources.Load(x+"");
      //  probeComponent.customBakedTexture = t;
    }
}


#if UNITY_EDITOR

public class MakeLight
{ 

    [MenuItem("AutoSystem/LightMap")]
    static void Update()
    {
        Debug.Log("Start Make Light Map" + System.DateTime.Now.ToString());
        MkDir("Assets/Resources");
        MkDir("Assets/Resources/LightMaps");

        
        var dh = TimeData.clock.hour;
        var dm = TimeData.clock.min;

        for (int h = 0; h < 24; h++)
            for (int m = 0; m < 60; m++)
            {
                var x = h.ToString().PadLeft(2, '0') + "h" + m.ToString().PadLeft(2, '0') + "m";
                TimeData.clock.hour = h;
                TimeData.clock.min = m;
                
                Export_maps(x);
                EditorUtility.DisplayProgressBar("Make Light Map", "map:" + x.Replace("-", "時") + "分", (h * 60 + m) * 1f / 1440);
            }

        TimeData.clock.hour = dh;
        TimeData.clock.min = dm;

        Debug.Log("Finish Make Light Map" + System.DateTime.Now.ToString());
        EditorUtility.ClearProgressBar();
    }

    static void Export_maps(string c)
    {
        string path = "Assets/Resources/LightMaps/" + c;
        MkDir(path);
        AssetDatabase.CreateAsset(GameObject.Instantiate(LightmapSettings.lightProbes), path + "/Lightprobe.asset");

        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理.
            if (o.activeInHierarchy)
            {
                var co = o.GetComponent<ReflectionProbe>();

                if (co != null)
                {
                   // uint cc = o.GetComponent<ReflectionProbeTag>().TAG;

                    //Texture2D.  = co.texture;
                     
                }
            }
        }
    }

    static void MkDir(string c)
    {
        if (!Directory.Exists(c))
            Directory.CreateDirectory(c);
    }
}

#endif



//http://corevale.com/unity/6307


