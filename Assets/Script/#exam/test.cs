using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{  
    public Camera cam; 

    private void Awake()
    {
        _MainCamera.SetCamera(cam);
    }

    void Update()
    {
        
    }

}

/* 
 １．綺麗なグラフィックス
 ２．操作性の高いシステム
 ３．事前描画による軽量化!! 
 ４．雲 
*/
