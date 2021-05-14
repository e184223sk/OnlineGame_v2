using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonobitEngine.MonoBehaviour
{
    public string WeaponName;
    public Texture2D icon;
    public string animations;
    public bool useAnimetion;
    public string userID;
    public Vector3 layoutAngle;
    public enum HasHand { LEFT, RIGHT }
    public HasHand hand;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
