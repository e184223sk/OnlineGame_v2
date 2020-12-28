using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public static class Key
{
    public static SelectGamePad gamePad;
    
    
    //================================================   [KEYBOARD]　　　　　[PS4]　　　　　　　　　XBOX360
    public static readonly GamePad_TB A  = new GamePad_TB(KeyCode.Z, KeyCode.JoystickButton1,  KeyCode.JoystickButton0); 
    public static readonly GamePad_TB B  = new GamePad_TB(KeyCode.X, KeyCode.JoystickButton2,  KeyCode.JoystickButton1); 
    public static readonly GamePad_TB X  = new GamePad_TB(KeyCode.C, KeyCode.JoystickButton0,  KeyCode.JoystickButton2); 
    public static readonly GamePad_TB Y  = new GamePad_TB(KeyCode.V, KeyCode.JoystickButton3,  KeyCode.JoystickButton3); 
    public static readonly GamePad_TB FL = new GamePad_TB(KeyCode.B, KeyCode.JoystickButton8,  KeyCode.JoystickButton6); 
    //public static readonly GamePad_TB FC = new GamePad_TB(KeyCode.N, KeyCode.JoystickButton14, KeyCode.JoystickButton15); //X
    public static readonly GamePad_TB FR = new GamePad_TB(KeyCode.M, KeyCode.JoystickButton9, KeyCode.JoystickButton7);

    //=================================================================================
    public static readonly GamePad_JS JoyStickL = new GamePad_JS("", "");
    public static readonly GamePad_JS JoyStickR = new GamePad_JS("", "");
    
    //================================================================
    public static readonly GamePad_Trigger _L  = new GamePad_Trigger();
    public static readonly GamePad_Trigger _LT = new GamePad_Trigger();
    public static readonly GamePad_Trigger _R  = new GamePad_Trigger();
    public static readonly GamePad_Trigger _RT = new GamePad_Trigger();
}


public enum SelectGamePad
{
    PS4,
    XBOX360
}


public class GamePad_Trigger
{
    

}

public class GamePad_JS
{
    string _x, _y;
    public GamePad_JS(string x, string y)
    {
        _x = x;
        _y = y;
    }

    public Vector2 Get
    {
        get
        {
            return new Vector2(Input.GetAxis(_x), Input.GetAxis(_y));
        }
    }

    public bool Push
    {
        get
        {
            return false;
        }
    }

}

public class GamePad_TB
{
    KeyCode ps4, xbox360, keyboard;
    public GamePad_TB(KeyCode keyboard, KeyCode ps4, KeyCode xbox360)
    {
        this.ps4 = ps4;
        this.xbox360 = xbox360;
        this.keyboard = keyboard;
    }

    public bool Press
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4    : return Input.GetKey(keyboard) || Input.GetKey(ps4);
                case SelectGamePad.XBOX360: return Input.GetKey(keyboard) || Input.GetKey(xbox360);
            }
            return false;
        }
    }

    public bool Down
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return Input.GetKeyDown(keyboard) || Input.GetKeyDown(ps4);
                case SelectGamePad.XBOX360: return Input.GetKeyDown(keyboard) || Input.GetKeyDown(xbox360);
            }
            return false;
        }
    }

    public bool Up
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return Input.GetKeyUp(keyboard) || Input.GetKeyUp(ps4);
                case SelectGamePad.XBOX360: return Input.GetKeyUp(keyboard) || Input.GetKeyUp(xbox360);
            }
            return false;
        }
    }
}

/*
 
     
     
     */