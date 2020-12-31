using UnityEngine;

public class _MainCamera 
{ 
    public static void SetCamera(Camera c)
    { 
        LOD_Manager._camera = c.transform;
        Wave.wave.targetCamera = c;
    } 
}
