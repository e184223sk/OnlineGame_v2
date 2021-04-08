using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setter : MonoBehaviour
{
    public bool Made;
    public int madeCount;
    public float x, z;

    public string[] objPath; 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(x, 30, z));
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, Vector3.one);
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, Vector3.one * 0.4f);
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, Vector3.one * 0.2f);
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, new Vector3(x, 0.5f, 1));
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, new Vector3(x, 0.5f, 0.4f));
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, new Vector3(1, 0.5f, z));
        Gizmos.DrawWireCube(transform.position + Vector3.up * 5, new Vector3(0.4f, 0.5f, z));
        if (Made)
        {
            List<GameObject> dd = new List<GameObject>();
            Made = false ;


            for (int v = 0; v < madeCount; v++)
            { 
                RaycastHit hit;

                Vector3 distance = Vector3.positiveInfinity;
                do
                {
                    float X = transform.position.x + Random.Range(-x,x) ;
                    float Z = transform.position.z + Random.Range(-z,z) ;
                    Ray ray = new Ray(new Vector3(X, 1000, Z), Vector3.down);
                    Physics.Raycast(ray, out hit, 1200);
                    if (hit.transform != null)
                    {

                    }
                }
                while (hit.transform != null && dd.Contains(hit.transform.gameObject) && Mathf.Abs(hit.transform.position.x - transform.position.x) < x / 2 && Mathf.Abs(hit.transform.position.z - transform.position.z)  < z / 2);
                if (objPath.Length < 0) return;

                string objName = "#Release#/" + objPath[Random.Range(0, objPath.Length)];
                dd.Add(Instantiate(Resources.Load(objName)as GameObject, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0)));
            }

        }

    }
}
