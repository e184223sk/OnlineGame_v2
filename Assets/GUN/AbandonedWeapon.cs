using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedWeapon : MonoBehaviour
{
    Color ringColor;
    public Color HittingTimeColor = new Color(1, 0, 0.5f, 1);
    public Color NonTimeRingColor = new Color(0.35f, 0.8353f, 1);
    public GameObject ring;
    MeshRenderer[] ringC;
    public GameObject have;
    public float tmr = 3;
    const float tmrV =0.1f;
    void Start()
    {
        tmr = tmrV;
        ringC = new MeshRenderer[6];
        for (int r = 0; r < ringC.Length; r++)
            ringC[r] = ring.transform.Find("r" + r).GetComponent<MeshRenderer>();
    }
     
    void Update()
    {
        if (have == null) Destroy(gameObject);
        ringColor = NonTimeRingColor;
        ring.transform.LookAt(Camera.main.transform);
        for (int c = 0; c < 3; c++)
            ringC[c]?.transform.Rotate(Vector3.up * Time.deltaTime * 0.5f * 110, Space.Self);
        for (int c = 3; c < 6; c++)
            ringC[c]?.transform.Rotate(Vector3.up * Time.deltaTime * -0.5f * 110, Space.Self); 
        have?.transform?.Rotate(Vector3.up * Time.deltaTime / 2 * 260, Space.World);

        
        if (tmr > 0) tmr -= Time.deltaTime; else tmr = 0; 

        foreach (var i in ringC)
            i.material.SetColor("_EmissionColor", tmr > 0 ? HittingTimeColor : NonTimeRingColor);
    }
     

    private void OnTriggerStay(Collider other)
    { 
        if (other.transform.root.gameObject.GetComponent<PLAYERS>() != null)
        {  
            tmr = tmrV;
            //UI表示 ***
            if (Key.FL.Down)
            { 
                GameObject xx = other.transform.root.gameObject.GetComponent<WeaponSelector>().SetWeapon(have);
               // have = xx;
                have.transform.parent = transform;
                have.transform.localPosition = Vector3.zero;
            }
        }
    }
}
