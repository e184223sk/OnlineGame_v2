using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene_Ctrl : MonoBehaviour
{
    public enum SELECTMODE {GameMode ,Tutorial, Option, MakeAvator, BackTitle }
    public SELECTMODE mode, Debug_;
    public float cnt, cnt2;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetMode = Debug_;
        Debug_ = mode;
        SetUI();
    }

    SELECTMODE SetMode
    {
        set
        {
            if (mode != value)
            { 
                cnt = 0;
            } 
        }
    }

    void SetUI()
    {
        SetButton();
        SetText();
    }

    void SetButton()
    {
        cnt += Time.deltaTime;
        cnt2 += Time.deltaTime/2;
        var sc = Color.HSVToRGB(cnt2 % 1f, 0.5f , (cnt % 1f) *0.3f + 0.7f);
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
