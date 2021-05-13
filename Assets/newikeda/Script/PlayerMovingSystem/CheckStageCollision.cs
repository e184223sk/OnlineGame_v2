﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStageCollision : MonoBehaviour
{
    public float searchheight; 
    public float searchArea;
    public int   checkInCircle;
    public int   checkToYaxis;
    public float startRadius;
    public float[] data;

    void Start()
    {

    }
    

    void Update()
    {
        Vector3[][] v = new Vector3[checkToYaxis][]; 
        data = new float[checkToYaxis* checkInCircle];

        for (int y = 0; y < checkToYaxis; y++)
        {
            v[y] = GetPoint;
            for (int x = 0; x < checkInCircle; x++)
            {
                Vector3[] c = GetSandE(v[y][x], searchheight / checkToYaxis * y);
                data[y * checkInCircle + x] = 10000;
                RaycastHit hit;
                if (Physics.Raycast( c[0], (c[1] - c[0]), out hit, searchArea))
                { 
                    if (hit.transform.root != transform.root)
                    {  
                        data[x] = hit.distance;
                    }
                } 
            }
        }

       
    }
     

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!UnityEditor.EditorApplication.isPlaying) Update();
        
        Vector3[] p = GetPoint;
        for (int y = 0; y < checkToYaxis; y++)
        { 
            for (int x = 0; x < p.Length; x++)
            {
                Vector3[] c = GetSandE(p[x], searchheight/ checkToYaxis * y);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(c[0], c[1]);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(c[0], 0.012f);
                Gizmos.DrawWireSphere(c[1], 0.012f);
            }
        }
    }
    #endif

    


    Vector3[] GetSandE(Vector3 p, float r)
    {
        return new Vector3[] 
        {
            p + r * Vector3.up,
            p + r * Vector3.up + (p - transform.position) * searchArea
        };
    }
    
    
    Vector3[] GetPoint
    {
        get
        {
            List<Vector3> p = new List<Vector3>();
            for (int x = 0; x < checkInCircle; x++)
                p.Add(transform.position + startRadius * ( new Vector3 ( Mathf.Sin(2f / checkInCircle * x * Mathf.PI), 0, Mathf.Cos(2f / checkInCircle * x * Mathf.PI))));
            return p.ToArray();
        } 
    } 


}
