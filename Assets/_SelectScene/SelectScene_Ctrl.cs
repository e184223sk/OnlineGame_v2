using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene_Ctrl : MonoBehaviour
{
     
    public enum SELECTMODE {GameMode ,Tutorial, Option, MakeAvator, BackTitle }
    public SELECTMODE mode ;
    public float cnt, cnt2;
    
    [SerializeField, Range(0, 1f)]
    float WhiteLevel = 1, ColorSpeed = 0.1f, alphaSpeed = 1, ColorLevel;
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
                case SELECTMODE.GameMode  : SceneLoader.Load("GO_Scene"); break;
                case SELECTMODE.Tutorial  : SceneLoader.Load("TutorialScene"); break;
                case SELECTMODE.MakeAvator: SceneLoader.Load("MakeAvatorScene"); break;
                case SELECTMODE.Option    : SceneLoader.Load("OptionScene"); break;
                case SELECTMODE.BackTitle : SceneLoader.Load("TitleScene"); break;
            } 
        }
        if (Key.B.Down)
        {
            mode = SELECTMODE.BackTitle;
            ToneSE(back);
        }

        //移動
        var d = Key.JoyStickL.Get;
        var dl = mode;

        var sens = 0.2f;
        string c = "";
        if (d.x > sens) c += "p"; else if (d.x < -sens) c += "m"; else c += "n";
        if (d.y > sens) c += "p"; else if (d.y < -sens) c += "m"; else c += "n";


        if (c == "nn")
        {
            flag = true;
        }
        else if(flag)
        {
            switch (mode)
            {
                //SELECTMODE.GameMode ------------------------------
                case SELECTMODE.GameMode:
                    /* */if (c == "pp") mode = SELECTMODE.Option;
                    else if (c == "mp") mode = SELECTMODE.Tutorial;
                    else if (c == "pm") mode = SELECTMODE.BackTitle;
                    else if (c == "mm") mode = SELECTMODE.MakeAvator;
                    else flag = true;
                    break; 

                //SELECTMODE.Tutorial ------------------------------
                case SELECTMODE.Tutorial:
                    /* */if (c[0] == 'p' && c[1] == 'm') mode = SELECTMODE.Option;
                    else if (c[0] != 'p' && c[1] == 'm') mode = SELECTMODE.MakeAvator;
                    else flag = true;
                    break;


                //SELECTMODE.MakeAvator ------------------------------
                case SELECTMODE.MakeAvator:
                    /* */if(c[0] == 'p' && c[1] != 'p') mode = SELECTMODE.BackTitle;
                    else if(c[0] == 'p' && c[1] == 'p') mode = SELECTMODE.Tutorial;
                    else flag = true;
                    break;

                //SELECTMODE.Option ------------------------------
                case SELECTMODE.Option:
                    if (c[0] == 'm' && c[1] != 'm') mode = SELECTMODE.GameMode;
                    else if (c[0] != 'm' && c[1] == 'm') mode = SELECTMODE.BackTitle;
                    else flag = true;
                    break;


                //SELECTMODE.BackTitle ------------------------------
                case SELECTMODE.BackTitle:
                    /* */if (c[0] == 'm' && c[1] != 'p') mode = SELECTMODE.GameMode;
                    else if (c[0] != 'm' && c[1] == 'p') mode = SELECTMODE.Option; 
                    else flag = true;
                    break;
                
            }

        }


        if (dl != mode)
        { 
            flag = false;
            ToneSE(select);
        }

        SetButton();
        SetText();
    } 
     
    public float SC_H,SC_S, SC_V;
    void SetButton()
    {
        cnt += Time.deltaTime * alphaSpeed;
        cnt2 += Time.deltaTime* ColorSpeed;
        SC_H = (Mathf.Sin(cnt2) + 1)/2;
        SC_S = ColorLevel;
        SC_V = (Mathf.Sin(cnt) * (1 - WhiteLevel) + WhiteLevel + 1) / 2;
        var sc = Color.HSVToRGB(SC_H, SC_S, SC_V);
        var nc = new Color(0.4f, 0.4f, 0.4f, 1); 
        Button_GameMode.color = mode == SELECTMODE.GameMode ? sc : nc;
        Button_Tutorial.color = mode == SELECTMODE.Tutorial ? sc : nc;
        Button_Option.color = mode == SELECTMODE.Option ? sc : nc;
        Button_MakeAvator.color = mode == SELECTMODE.MakeAvator ? sc : nc;
        Button_BackTitle.color = mode == SELECTMODE.BackTitle ? sc : nc;
    }

    void SetText()
    {
        switch (mode)
        {
            case SELECTMODE.GameMode  : DiscriptionTextArea.texture = Text_GameMode;   break;
            case SELECTMODE.Tutorial  : DiscriptionTextArea.texture = Text_Tutorial;   break;
            case SELECTMODE.Option    : DiscriptionTextArea.texture = Text_Option;     break;
            case SELECTMODE.MakeAvator: DiscriptionTextArea.texture = Text_MakeAvator; break;
            case SELECTMODE.BackTitle : DiscriptionTextArea.texture = Text_BackTitle;  break;
        }
    }

}
