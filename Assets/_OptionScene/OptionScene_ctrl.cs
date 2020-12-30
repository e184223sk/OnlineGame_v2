using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionScene_ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetGraphic(int v) => QualitySettings.SetQualityLevel(v > 4 ? 4 : (v< 0 ? 0 : 1));
}
