﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class netinit : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    GameObject obj;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) MonobitNetwork.Instantiate("Sphere", new Vector3(0, 0, -5), Quaternion.identity, 0);
    }
}
