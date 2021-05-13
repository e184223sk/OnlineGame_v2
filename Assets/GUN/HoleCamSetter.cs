using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCamSetter : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        HoleDirectionSetter.camera = cam;
        Destroy(this);    
    }
     
}
