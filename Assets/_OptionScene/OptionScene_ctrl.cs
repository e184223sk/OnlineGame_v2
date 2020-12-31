using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScene_ctrl : MonoBehaviour
{
    [SerializeField]
    Mode mode;

    [SerializeField, Space(20)]
    Transform
       tag_bgm;
   [SerializeField ]
    Transform 
        tag_se,
        tag_voice,
        tag_master,
        tag_joystick,
        tag_selectGamepad,
        tag_graphicQuality,
        tag_invertX,
        tag_invertY,
        tag_LOAD,
        tag_SAVE,
        tag_RETURN;

    [SerializeField,Space(20)]
    Transform _bgm;
    [SerializeField]
    Transform  _se, _voice, _master, _joystick, _invertX, _invertY;

    [SerializeField]
    RawImage Button_SAVE, Button_LOAD, Button_RETURN;

    [Space(30)]
    [SerializeField]
    UI_ENUMTYPE SelectGamepad, GraphicQuality;

    [SerializeField]
    ON_OFF InvertX, InvertY;

    [SerializeField]
    RawImage[] bgm, se, voice, master, joystick;

    [SerializeField]
    RawImage
         text_bgm,
         text_se,
         text_voice,
         text_master,
         text_joystick,
         text_selectGamepad,
         text_graphicQuality,
         text_invertX,
         text_invertY,
         text_LOAD,
         text_SAVE,
         text_RETURN;
     

    float tagColor,tagCnt;
    bool idol,LCM,LCP,P;

    void Start()
    {
        ConfigData_Manager.LOAD();
        bgm = InitBar(_bgm);
        se = InitBar(_se);
        voice = InitBar(_voice);
        master = InitBar(_master);
        joystick = InitBar(_joystick);
        InvertX = InitBOOL(_invertX);
        InvertY = InitBOOL(_invertY);
    }


    void Update()
    {
        //Get Input Data --------------------
        var border = 0.3f; 
        Vector2 c = Key.JoyStickL;
        bool MV = c.y < -border;
        bool MH = c.x < -border;
        bool PV = c.y > border;
        bool PH = c.x > border;
        bool CP = Key.B.Press;
        bool CM = Key.A.Press;

       
        //Select and Change Select-----------
        if (idol)
        { 
            switch (mode)
            {
                case Mode.BGM:
                    if (PV) mode = Mode.SE;
                    if (MV) mode = Mode.SAVE;
                    if (PH || MH)
                        mode = Mode.JOYSTICK;
                    break;

                case Mode.SE:
                    if (PV) mode = Mode.BGM;
                    if (MV) mode = Mode.VOICE;
                    if (PH || MH)
                        mode = Mode.SELECTGAMEPAD;
                    break;

                case Mode.VOICE:
                    if (PV) mode = Mode.SE;
                    if (MV) mode = Mode.MASTER;
                    if (PH || MH)
                        mode = Mode.GRAPHICQUALITY;
                    break;


                case Mode.MASTER:
                    if (PV) mode = Mode.VOICE;
                    if (MV) mode = Mode.LOAD;
                    if (PH) mode = Mode.INVERT_X;
                    if (MH) mode = Mode.INVERT_Y;
                    break;



                case Mode.JOYSTICK:
                    if (PV) mode = Mode.RETURN;
                    if (MV) mode = Mode.SELECTGAMEPAD;
                    if (PH || MH) mode = Mode.BGM;
                    break;

                case Mode.SELECTGAMEPAD:
                    if (PV) mode = Mode.JOYSTICK;
                    if (MV) mode = Mode.GRAPHICQUALITY;
                    if (PH || MH) mode = Mode.SE;
                    break;

                case Mode.GRAPHICQUALITY:
                    if (PV) mode = Mode.SELECTGAMEPAD;
                    if (MV) mode = Mode.INVERT_X;
                    if (PH || MH) mode = Mode.VOICE;
                    break;

                case Mode.INVERT_X:
                    if (PV) mode = Mode.GRAPHICQUALITY;
                    if (MV) mode = Mode.INVERT_Y;
                    if (PH || MH) mode = Mode.MASTER;
                    break;


                case Mode.INVERT_Y:
                    if (PV) mode = Mode.INVERT_X;
                    if (PH || MH) mode = Mode.MASTER;
                    if (MV)
                        mode = GamePad_JS.InvertY ? Mode.LOAD : Mode.RETURN;
                    break;



                case Mode.LOAD:
                    if (PV) mode = Mode.MASTER;
                    if (MV) mode = Mode.BGM;
                    if (PH) mode = Mode.SAVE;
                    if (MH) mode = Mode.RETURN;
                    break;



                case Mode.RETURN:
                    if (PV) mode = Mode.INVERT_Y;
                    if (MV) mode = Mode.JOYSTICK;
                    if (PH) mode = Mode.LOAD;
                    if (MH) mode = Mode.SAVE;
                    break;



                case Mode.SAVE:
                    if (PV) mode = Mode.MASTER;
                    if (MV) mode = Mode.BGM;
                    if (PH) mode = Mode.RETURN;
                    if (MH) mode = Mode.LOAD;
                    break;
            }
            idol = false;
        }
        else
        {
            idol = (Mathf.Abs(c.x) + Mathf.Abs(c.y)) / 2 < border;
        }

        if( ((LCP != CP) && CP) || ((LCM != CM) && CM) )
            switch (mode)
            {
                case Mode.BGM: 
                    if (CP) AudioSystem.BGM += 0.1f;
                    if (CM) AudioSystem.BGM -= 0.1f; 
                    break;

                case Mode.SE:
                    if (CP) AudioSystem.SE += 0.1f;
                    if (CM) AudioSystem.SE -= 0.1f;
                    break;

                case Mode.VOICE:
                    if (CP) AudioSystem.VOICE += 0.1f;
                    if (CM) AudioSystem.VOICE -= 0.1f;
                    break;


                case Mode.MASTER:
                    if (CP) AudioSystem.MASTER += 0.1f;
                    if (CM) AudioSystem.MASTER -= 0.1f;
                    break;



                case Mode.JOYSTICK:
                    if (CP) GamePad_JS.sensivirity += 0.1f;
                    if (CM) GamePad_JS.sensivirity -= 0.1f;
                    break;

                case Mode.SELECTGAMEPAD: 
                    if (CP) SelectGamepad.Select++;
                    if (CM) SelectGamepad.Select--; 
                    break;

                case Mode.GRAPHICQUALITY:
                    if (CP) GraphicQuality.Select++;
                    if (CM) GraphicQuality.Select--;
                    break;

                case Mode.INVERT_X:  
                    if (CP || CM) GamePad_JS.InvertX = !GamePad_JS.InvertX;
                    break;
                 
                case Mode.INVERT_Y:
                    if (CP || CM) GamePad_JS.InvertY = !GamePad_JS.InvertY;
                    break;
                 
                case Mode.LOAD: 
                    if (CP) ConfigData_Manager.LOAD();
                    if (CM) mode = Mode.RETURN;
                    break;
                 
                case Mode.SAVE:
                    if (CP) ConfigData_Manager.SAVE();
                    if (CM) mode = Mode.RETURN; 
                    break;
                 
                case Mode.RETURN:
                    if (CP) SceneLoader.Load("SelectScene");
                    if (CM) mode = Mode.RETURN;
                    break;

            }

        //Limit Value----------------------------------------------
        if (GamePad_JS.sensivirity < 0.1f)
            GamePad_JS.sensivirity = 0.1f;
        
        //Update Drawing UI----------------------------------------
        SetBarUI(bgm, AudioSystem.BGM);
        SetBarUI(se, AudioSystem.SE);
        SetBarUI(voice, AudioSystem.VOICE);
        SetBarUI(master, AudioSystem.MASTER);
        SetBarUI(joystick, GamePad_JS.sensivirity);
        SetONOFF(InvertX, GamePad_JS.InvertX);
        SetONOFF(InvertY, GamePad_JS.InvertY); 
        if (mode != Mode.SAVE) Button_SAVE.color = Color.gray;
        if (mode != Mode.LOAD) Button_LOAD.color = Color.gray;
        if (mode != Mode.RETURN) Button_RETURN.color = Color.gray;

        //Draw Select UI -------------------------------------------
        tagColor = Mathf.Sin(Time.time*2);
        var cc = (tagColor+1)/4 + 0.5f;
        tag_bgm.GetComponent<RawImage>().color = mode == Mode.BGM ? new Color(cc, cc, cc,1) : Color.gray;
        tag_se.GetComponent<RawImage>().color = mode == Mode.SE ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_voice.GetComponent<RawImage>().color = mode == Mode.VOICE ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_master.GetComponent<RawImage>().color = mode == Mode.MASTER ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_joystick.GetComponent<RawImage>().color = mode == Mode.JOYSTICK ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_selectGamepad.GetComponent<RawImage>().color = mode == Mode.SELECTGAMEPAD ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_graphicQuality.GetComponent<RawImage>().color = mode == Mode.GRAPHICQUALITY ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_invertX.GetComponent<RawImage>().color = mode == Mode.INVERT_X ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_invertY.GetComponent<RawImage>().color = mode == Mode.INVERT_Y ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_LOAD.GetComponent<RawImage>().color = mode == Mode.LOAD ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_SAVE.GetComponent<RawImage>().color = mode == Mode.SAVE ? new Color(cc, cc, cc, 1) : Color.gray;
        tag_RETURN.GetComponent<RawImage>().color = mode == Mode.RETURN ? new Color(cc, cc, cc, 1) : Color.gray;

        //Draw Discription Text ---------------------------------------

        text_bgm.enabled = false;
        text_se.enabled = false;
        text_voice.enabled = false;
        text_master.enabled = false;
        text_joystick.enabled = false;
        text_selectGamepad.enabled = false;
        text_graphicQuality.enabled = false;
        text_invertX.enabled = false;
        text_invertY.enabled = false;
        text_LOAD.enabled = false;
        text_SAVE.enabled = false;
        text_RETURN.enabled = false;

        switch (mode)
        {
            case Mode.BGM: text_bgm.enabled = true; break; 
            case Mode.SE: text_se.enabled = true; break;
            case Mode.VOICE: text_voice.enabled = true; break;
            case Mode.MASTER: text_master.enabled = true; break;
            case Mode.JOYSTICK: text_joystick.enabled = true; break;
            case Mode.SELECTGAMEPAD: text_selectGamepad.enabled = true; break;
            case Mode.GRAPHICQUALITY: text_graphicQuality.enabled = true; break;
            case Mode.INVERT_X: text_invertX.enabled = true; break;
            case Mode.INVERT_Y: text_invertY.enabled = true; break;
            case Mode.LOAD: text_LOAD.enabled = true; break;
            case Mode.RETURN: text_RETURN.enabled = true; break;
            case Mode.SAVE: text_SAVE.enabled = true; break;
        }

        //FINISH TASK
        LCM = CM;
        LCP = CP;

    }
     
    



    public enum Mode
    {
        BGM,
        SE,
        VOICE,
        MASTER,
        JOYSTICK,
        SELECTGAMEPAD,
        GRAPHICQUALITY,
        INVERT_X,
        INVERT_Y,
        SAVE,
        LOAD,
        RETURN
    }

    void SetGraphic(int v) => QualitySettings.SetQualityLevel(v > 4 ? 4 : (v < 0 ? 0 : 1));

    RawImage[] InitBar(Transform a)
    {
        RawImage[] d = new RawImage[10];
        for (var f = 1; f <= 10; f++)
            d[f-1] = a.Find(f.ToString()).GetComponent<RawImage>();
        return d;
    }

    public void SetBarUI(RawImage[] t, float value_)
    {
        int h = (int)(value_ * 10);
        for (int r = 1; r <= 10; r++) 
            t[r - 1].color = r <= h ? Color.white : new Color(0.3f, 0.3f, 0.35f);
    }

    ON_OFF InitBOOL(Transform i) =>
         new ON_OFF()
         {
             off = i.Find("OFF").GetComponent<RawImage>(),
             on = i.Find("ON").GetComponent<RawImage>()
         };

    [System.Serializable]
    struct UI_ENUMTYPE
    {
        public RawImage screen;
        public RawImage Back,Next;
        public Texture2D[] list;
        int g;
        public int Select
        {
            get => g;
            set
            {
                g = value;
                if (g < 0)
                    g = 0;
                else if(g >= list.Length)
                    g = list.Length - 1;

                Back.color = g == 0 ? Color.gray : Color.white;
                Next.color = g == list.Length - 1 ? Color.gray : Color.white;
                screen.texture = list[g];
            }
        }
    }

    void SetONOFF(ON_OFF i, bool x)
    {
        i.on.color  =  x ? Color.white : new Color(0.3f, 0.3f, 0.35f);
        i.off.color = !x ? Color.white : new Color(0.3f, 0.3f, 0.35f);
    }

    [System.Serializable]
    struct ON_OFF
    {
        public RawImage on, off;
    }
     
}

  