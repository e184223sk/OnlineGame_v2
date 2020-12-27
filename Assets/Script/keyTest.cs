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
        if (Key.A.Press) Debug.Log("A Press");
        if (Key.B.Press) Debug.Log("B Press");
        if (Key.X.Press) Debug.Log("X Press");
        if (Key.Y.Press) Debug.Log("Y Press");
        if (Key.FL.Press) Debug.Log("FL Press"); 
        if (Key.FR.Press) Debug.Log("FR Press");


        if (Key.A.Down) Debug.Log("A Down");
        if (Key.B.Down) Debug.Log("B Down");
        if (Key.X.Down) Debug.Log("X Down");
        if (Key.Y.Down) Debug.Log("Y Down");
        if (Key.FL.Down) Debug.Log("FL Down"); 
        if (Key.FR.Down) Debug.Log("FR Down");


        if (Key.A.Up) Debug.Log("A Up");
        if (Key.B.Up) Debug.Log("B Up");
        if (Key.X.Up) Debug.Log("X Up");
        if (Key.Y.Up) Debug.Log("Y Up");
        if (Key.FL.Up) Debug.Log("FL Up"); 
        if (Key.FR.Up) Debug.Log("FR Up");

        //-----------------------------------------------

        //-----------------------------------------------

    }
}
