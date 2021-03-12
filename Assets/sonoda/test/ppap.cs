using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppap : MonoBehaviour
{

    int a = 19;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) a = Random.Range(0, 100);
        if(Input.GetKeyDown(KeyCode.W))  Debug.Log(a);
    }
}
