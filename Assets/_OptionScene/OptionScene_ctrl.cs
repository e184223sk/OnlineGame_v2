using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScene_ctrl : MonoBehaviour
{

    
    [SerializeField]
    Mode mode;

    [SerializeField, Range(0.1f, 3)]
    float MoveTimeInterval = 1;


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

    public AudioSource Se;
    [SerializeField]
    AudioClip next, back, move;

    public AudioSource VoiceSE;
    float tagColor,tagCnt;
    bool idol,LCM,LCP,P;
    float flagTrueTime;


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
        var border = 0.1f; 
        Vector2 c = Key.JoyStickL.Get/GamePad_JS.sensivirity;
        if (GamePad_JS.InvertX) c.x *= -1;
        if (GamePad_JS.InvertY) c.y *= -1;
        var mL = mode;
        string xx = c.x > border ? "R" : (c.x < -border ? "L" : "N");
        float tx = Mathf.Abs(c.x);
        string xy = c.y > border ? "T" : (c.y < -border ? "B" : "N");
        float ty = Mathf.Abs(c.y);
        string ccc = tx > ty ? xx : xy;
        bool CP = Key.B.Down;
        bool CM = Key.A.Down;

        //Select and Change Select-----------
        if (idol)
        {
            idol = false;
            flagTrueTime = 0;
            switch (mode)
            {
                case Mode.BGM:
                    switch (ccc)
                    {
                        case "B": mode = Mode.SE; break; 
                        case "R": mode = Mode.JOYSTICK; break;
                        default: idol = true; break;
                    } 
                    break;


                case Mode.SE:
                    switch (ccc)
                    {
                        case "T": mode = Mode.BGM; break;
                        case "B": mode = Mode.VOICE; break; 
                        case "R": mode = Mode.SELECTGAMEPAD; break;
                        default: idol = true; break;
                    }
                    break;

                case Mode.VOICE:
                    switch (ccc)
                    {
                        case "T": mode = Mode.SE; break;
                        case "B": mode = Mode.MASTER; break;
                        case "R": mode = Mode.GRAPHICQUALITY; break;
                        default: idol = true; break;
                    }
                    break;


                case Mode.MASTER:
                    switch (ccc)
                    {
                        case "T": mode = Mode.VOICE; break;
                        case "B": mode = Mode.LOAD; break; 
                        case "R": mode = Mode.INVERT_Y; break;
                        default: idol = true; break;
                    }
                    break;



                case Mode.JOYSTICK:
                    switch (ccc)
                    {
                        case "B": mode = Mode.SELECTGAMEPAD; break;
                        case "L": mode = Mode.BGM; break;
                        default: idol = true; break;
                    }
                    break;

                case Mode.SELECTGAMEPAD:

                    switch (ccc)
                    {
                        case "T": mode = Mode.JOYSTICK; break;
                        case "B": mode = Mode.GRAPHICQUALITY; break;
                        case "L": mode = Mode.SE; break; 
                        default: idol = true; break;
                    }
                    break;

                case Mode.GRAPHICQUALITY:
                    switch (ccc)
                    {
                        case "T": mode = Mode.SELECTGAMEPAD; break;
                        case "B": mode = Mode.INVERT_X; break;
                        case "L": mode = Mode.VOICE; break;
                        default: idol = true; break;
                    }
                    break;

                case Mode.INVERT_X:
                    switch (ccc)
                    {
                        case "T": mode = Mode.GRAPHICQUALITY; break;
                        case "B": mode = Mode.INVERT_Y; break;
                        case "L": mode = Mode.VOICE; break;
                        default: idol = true; break;
                    }
                    break;


                case Mode.INVERT_Y:
                    switch (ccc)
                    {
                        case "T": mode = Mode.INVERT_X; break;
                        case "B": mode = Mode.RETURN; break;
                        case "L": mode = Mode.MASTER; break;
                        default: idol = true; break;
                    }
                    break;



                case Mode.LOAD:
                    switch (ccc)
                    {
                        case "T": mode = Mode.MASTER; break;
                        case "R": mode = Mode.SAVE; break;
                        default: idol = true; break;
                    }
                    break;



                case Mode.RETURN:
                    switch (ccc)
                    {
                        case "T": mode = Mode.INVERT_Y; break;
                        case "L": mode = Mode.SAVE; break;
                        default: idol = true; break;
                    }
                    break;



                case Mode.SAVE: 
                    switch (ccc)
                    {
                        case "T": mode = Mode.INVERT_Y; break;
                        case "L": mode = Mode.LOAD; break;
                        case "R": mode = Mode.RETURN; break;
                        default: idol = true; break;
                    }
                    break;
            } 
        }
        else
        {
            idol = Mathf.Abs(c.x) < border && Mathf.Abs(c.y) < border;
        }

        if (!idol)
        {
            flagTrueTime += Time.deltaTime;
            if (flagTrueTime >= MoveTimeInterval)
            {
                flagTrueTime = 0;
                idol = true;
            }
        }

        if ( ((LCP != CP) && CP) || ((LCM != CM) && CM) )
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
                    var vv = AudioSystem.VOICE;
                    if (CP) AudioSystem.VOICE += 0.1f;
                    if (CM) AudioSystem.VOICE -= 0.1f;
                    if (CP||CM) VoiceSE.Play();
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
                    if (CM)
                    {
                        PLAY_NEXT();
                        ConfigData_Manager.LOAD();
                    }
                    if (CP)
                    {
                        PLAY_BACK();
                        mode = Mode.RETURN;
                    }
                    break;

                case Mode.SAVE:
                    if (CM)
                    { 
                        PLAY_NEXT();
                        ConfigData_Manager.SAVE();
                    }
                    if (CP)
                    {
                        PLAY_BACK();
                        mode = Mode.RETURN;
                    } 
                    break;
                 
                case Mode.RETURN:
                    if (CM)
                    {
                        PLAY_BACK();
                        SceneLoader.Load("SelectScene");
                    }
                    
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
        if (mL != mode)
            PLAY_MOVE();

        if (mode != Mode.VOICE && (Key.B.Down || Key.A.Down) && !vFLAG)
        {
            vFLAG = true;
            Invoke("FLAG_V", 0.1f);
            PLAY_MOVE();
        }
    }

    void PLAY_NEXT() => TONE(next);
    void PLAY_BACK() => TONE(back);  
    void PLAY_MOVE() => TONE(move);  
    void TONE (AudioClip a) { Se.PlayOneShot(a);/* Se.clip = a; Se.Play();*/ }
    bool vFLAG;
    void FLAG_V() => vFLAG = false;

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

  