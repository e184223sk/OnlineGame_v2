﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigData_Manager 
{
    static List<Online_data> data = new List<Online_data>();

    public static void SAVE()
    { 
        //Option-------------------------------------------------------------------------------------
        data.Add(new Online_data() { tag = DATATAG.BGM_VOL, data = ((int)(AudioSystem.BGM * 10)).ToString()});
        data.Add(new Online_data() { tag = DATATAG.SE_VOL, data = ((int)(AudioSystem.SE * 10)).ToString() });
        data.Add(new Online_data() { tag = DATATAG.VOICE_VOL, data = ((int)(AudioSystem.VOICE * 10)).ToString() });
        data.Add(new Online_data() { tag = DATATAG.MASTER_VOL, data = ((int)(AudioSystem.MASTER * 10)).ToString() });
        data.Add(new Online_data() { tag = DATATAG.JoyStickSensivirity, data = ((int)(GamePad_JS.sensivirity * 10)).ToString() }); 
        data.Add(new Online_data() { tag = DATATAG.InvertX, data = GamePad_JS.InvertX ? "TRUE" : "FALSE" });
        data.Add(new Online_data() { tag = DATATAG.InvertY, data = GamePad_JS.InvertY ? "TRUE" : "FALSE" }); 
        data.Add(new Online_data() { tag = DATATAG.GraphicQuality, data = ConfigData.graphics.ToString() });
        data.Add(new Online_data() { tag = DATATAG.SelectGamePad, data = Key.select.ToString() });

        //Avator-------------------------------------------------------------------------------------

        //SAVE==============================================================================================
        Online_data.SaveList(NetData.Get__DATA_TXT, data);
    }
    public static void LOAD()
    { 
        data = Online_data.GetList("data.txt");

        //Option-------------------------------------------------------------------------------------
        AudioSystem.BGM = CASTFLOAT(Online_data.GetData(DATATAG.BGM_VOL, data));
        AudioSystem.SE = CASTFLOAT(Online_data.GetData(DATATAG.SE_VOL, data));
        AudioSystem.VOICE = CASTFLOAT(Online_data.GetData(DATATAG.VOICE_VOL, data));
        AudioSystem.MASTER = CASTFLOAT(Online_data.GetData(DATATAG.MASTER_VOL, data));
        GamePad_JS.sensivirity = CASTFLOAT(Online_data.GetData(DATATAG.JoyStickSensivirity, data));
        GamePad_JS.InvertX = Online_data.GetData(DATATAG.InvertX, data) == "TRUE";
        GamePad_JS.InvertY = Online_data.GetData(DATATAG.InvertY, data) == "TRUE";
        ConfigData.graphics = CASTINT(Online_data.GetData(DATATAG.GraphicQuality, data), ConfigData.graphics);
        Key.select = CASTINT(Online_data.GetData(DATATAG.SelectGamePad, data), Key.select);
        
        //Avator-------------------------------------------------------------------------------------
        
    }

    const int dd = 7;

    static float CASTFLOAT(string x)
    {
        x = x.Replace("[N/A]", dd.ToString());
        int c = dd;
        int.TryParse(x, out c);
        return c*0.1f;
    }

    static int CASTINT(string x, int y)
    {
        x = x.Replace("[N/A]", y.ToString()); 
        int.TryParse(x, out y);
        return y;
    }
}
 

public static class DATATAG
{
    //a-z,A-Z,0-9と_だけでかく

    //OPTIONWINDOWで設定するパラメータ----------------------------------
    public const string BGM_VOL   = "_Bgm_Volume";
    public const string SE_VOL    = "_Se_Volume";
    public const string VOICE_VOL = "_Voice_Volume";
    public const string MASTER_VOL = "_Master_Volume"; 
    public const string JoyStickSensivirity = "_Joystick_Sensivirity";
    public const string InvertX = "_InvertX";
    public const string InvertY = "_InvertY";
    public const string GraphicQuality = "_Graphic_Quality";
    public const string SelectGamePad = "_Select_GamePad";

    //アバター画面で設定するパラメータ-----------------------------------

    //その他-------------------------------------------------------------
}
