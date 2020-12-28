using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyTest : MonoBehaviour
{
    public SelectGamePad useGamepad;
    void Update()
    {
        Key.gamePad = useGamepad;
        
        //-------------------------------------------------
        if (Key.A.Press) Debug.Log("A/×  Press");
        if (Key.B.Press) Debug.Log("B/〇  Press");
        if (Key.X.Press) Debug.Log("X/□  Press");
        if (Key.Y.Press) Debug.Log("Y/△  Press");
        if (Key.FL.Press) Debug.Log("Back/Share  Press"); 
        if (Key.FR.Press) Debug.Log("Start/Option Press");


        if (Key.A.Down) Debug.Log("A/×  Down");
        if (Key.B.Down) Debug.Log("B/〇  Down");
        if (Key.X.Down) Debug.Log("X/□  Down");
        if (Key.Y.Down) Debug.Log("Y/△  Down");
        if (Key.FL.Down) Debug.Log("Back/Share Down"); 
        if (Key.FR.Down) Debug.Log("Start/Option Down");


        if (Key.A.Up) Debug.Log("A/×  Up");
        if (Key.B.Up) Debug.Log("B/〇  Up");
        if (Key.X.Up) Debug.Log("X/□  Up");
        if (Key.Y.Up) Debug.Log("Y/△  Up");
        if (Key.FL.Up) Debug.Log("Back/Share Up"); 
        if (Key.FR.Up) Debug.Log("Start/Option Up");

        //-----------------------------------------------

        //-----------------------------------------------

    }
}
