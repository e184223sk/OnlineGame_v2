using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene_Ctrl : MonoBehaviour
{ 
    [Space(10)]
    public SELECTMODE mode ;
    float cnt, cnt2;
    
    [Space(10)]
    [SerializeField, Range(0, 1f)]
    float WhiteLevel = 1;

    [SerializeField, Range(0, 1f)]
    float ColorSpeed = 0.1f, alphaSpeed = 1, ColorLevel;

    [Space(30)]
    [SerializeField]
    RawImage
        Button_GameMode;
    [SerializeField]
    RawImage  
        Button_Tutorial, 
        Button_Option, 
        Button_MakeAvator, 
        Button_BackTitle;

    [Space(30)]
    [SerializeField]
    RawImage DiscriptionTextArea;
    [SerializeField]
    Texture2D
        Text_GameMode,
        Text_Tutorial,
        Text_Option,
        Text_MakeAvator,
        Text_BackTitle;

    [Space(30)]
    [SerializeField]
    AudioSource Se;
    bool flag;
    [SerializeField]
    AudioClip next, back, select; 
    bool idol;
    int py;

    public float SC_H, SC_S, SC_V;

    [SerializeField, Range(0.1f, 2)]
    float MoveTimeInterval = 1;
    [SerializeField,Range(0,1f)]
    float border = 0.7f;
    float flagTrueTime;
    void Start()
    {
        mode = SELECTMODE.GameMode; 
    }

    void ToneSE(AudioClip a)
    {
        Se.clip = a;
        Se.Play();
    } 

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.IsFade) return;

        if (Key.A.Down)
        {
            ToneSE(next);
            switch (mode)
            {//^^
                case SELECTMODE.GameMode:
                    ToneSE(next);
                    SceneLoader.Load("GO_Scene");
                    break;

                case SELECTMODE.Tutorial  :
                    ToneSE(next);
                    SceneLoader.Load("TutorialScene");
                    break;

                case SELECTMODE.MakeAvator:
                    ToneSE(next);
                    SceneLoader.Load("MakeAvatorScene");
                    break;

                case SELECTMODE.Option    :
                    ToneSE(next);
                    SceneLoader.Load("OptionScene");
                    break;

                case SELECTMODE.BackTitle :
                    ToneSE(back);
                    SceneLoader.Load("TitleScene");
                    break;
            } 
        }
        if (Key.B.Down)
        {
            mode = SELECTMODE.BackTitle;
            ToneSE(back);
        }

        //移動
        //Get Input Data --------------------
        Vector2 d = Key.JoyStickL.Get / GamePad_JS.sensivirity;
        if (GamePad_JS.InvertX) d.x *= -1;
        if (GamePad_JS.InvertY) d.y *= -1;
        Debug.Log(d);
        string xx = d.x > border ? "R" : (d.x < -border ? "L" : "N");
        float tx = Mathf.Abs(d.x);
        string xy = d.y > border ? "T" : (d.y < -border ? "B" : "N");
        float ty = Mathf.Abs(d.y);
        string ccc = tx > ty ? xx : xy; 
        var dl = mode;

        Debug.Log(xx + ":" + xy + ":" + ccc);
        if (idol)
        {
            idol = false;
            flagTrueTime = 0;

            switch (mode)
            {
                case SELECTMODE.GameMode:
                    switch (ccc)
                    {
                        case "T": mode = xx == "R" ? SELECTMODE.Option : SELECTMODE.Tutorial; break;
                        case "B": mode = xx == "R" ? SELECTMODE.BackTitle : SELECTMODE.MakeAvator; break;
                        case "L": mode = xy == "T" ? SELECTMODE.Tutorial : SELECTMODE.MakeAvator; break;
                        case "R": mode = xy == "T" ? SELECTMODE.Option : SELECTMODE.BackTitle; break;
                        default: break;
                    }
                    break;

                case SELECTMODE.Tutorial:
                    switch (ccc)
                    {
                        case "B": mode = SELECTMODE.MakeAvator; break;
                        case "R": mode = SELECTMODE.GameMode; break;
                        default: break;
                    }
                    break;

                case SELECTMODE.MakeAvator:
                    switch (ccc)
                    {
                        case "T": mode = SELECTMODE.Tutorial; break;
                        case "R": mode = SELECTMODE.GameMode; break;
                        default: break;
                    }
                    break;

                case SELECTMODE.Option:
                    switch (ccc)
                    {
                        case "B": mode = SELECTMODE.BackTitle; break;
                        case "L": mode = SELECTMODE.GameMode; break;
                        default: break;
                    }
                    break;

                case SELECTMODE.BackTitle:
                    switch (ccc)
                    {
                        case "T": mode = SELECTMODE.Option; break;
                        case "L": mode = SELECTMODE.GameMode; break;
                        default: break;
                    }
                    break;

                default:
                    break;
            }
        }
        else
        {
            flagTrueTime += Time.deltaTime;
            if (flagTrueTime >= MoveTimeInterval)
            {
                flagTrueTime = 0;
                idol = true;
            }
            else
            { 
                idol = Mathf.Abs(d.x) < border && Mathf.Abs(d.y) < border;
            }
        }

        if (mode != dl)
        { 
            flag = false;
            ToneSE(select);
        }


        if (!idol)
        {
            
        }


        cnt += Time.deltaTime * alphaSpeed;
        cnt2 += Time.deltaTime * ColorSpeed;
        SC_H = (Mathf.Sin(cnt2) + 1) / 2;
        SC_S = ColorLevel;
        SC_V = (Mathf.Sin(cnt) * (1 - WhiteLevel) + WhiteLevel + 1) / 2;
        var sc = Color.HSVToRGB(SC_H, SC_S, SC_V);
        var nc = new Color(0.4f, 0.4f, 0.4f, 1);
        Button_GameMode.color = mode == SELECTMODE.GameMode ? sc : nc;
        Button_Tutorial.color = mode == SELECTMODE.Tutorial ? sc : nc;
        Button_Option.color = mode == SELECTMODE.Option ? sc : nc;
        Button_MakeAvator.color = mode == SELECTMODE.MakeAvator ? sc : nc;
        Button_BackTitle.color = mode == SELECTMODE.BackTitle ? sc : nc;

        switch (mode)
        {
            case SELECTMODE.GameMode: DiscriptionTextArea.texture = Text_GameMode; break;
            case SELECTMODE.Tutorial: DiscriptionTextArea.texture = Text_Tutorial; break;
            case SELECTMODE.Option: DiscriptionTextArea.texture = Text_Option; break;
            case SELECTMODE.MakeAvator: DiscriptionTextArea.texture = Text_MakeAvator; break;
            case SELECTMODE.BackTitle: DiscriptionTextArea.texture = Text_BackTitle; break;
        }
    }
     

}
 
public enum SELECTMODE { GameMode, Tutorial, Option, MakeAvator, BackTitle,None }
