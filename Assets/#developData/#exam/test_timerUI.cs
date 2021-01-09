using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_timerUI : MonoBehaviour
{
    public float time;
     

    // Update is called once per frame
    void Update()
    {
        var c = GetComponent<TimerUI>();
        c.time = time; 
    }
}
