using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene_Ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Key.A.Down)
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
