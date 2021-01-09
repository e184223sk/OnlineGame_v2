using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene_UI_Manager : MonoBehaviour
{
    public static GameScene_UI_Manager ui;

    [SerializeField,Range(0,1f)]
    float HpValue;

    public float hpValue
    {
        get
        {
            return HpValue;
        }
        set
        {
            HpValue = value > 1 ? 1 : (value < 0 ? 0 : value);
        }
    }

    [SerializeField]
    Gradient hpColor;

    Slider HP;

    Image hpImg;


    [SerializeField, Range(0,99999999),Space(15)]
    long MoneyValue;

    public long moneyValue
    {
        get
        {
            return MoneyValue;
        }

        set
        {
            MoneyValue = value > 999999999999 ? 999999999999 : (value < 0 ? 0 : value);
        }
    }


    [SerializeField]
    Gradient MoneyUiColor;

    [SerializeField]
    Texture2D[] num;

    //[SerializeField]
    RawImage[] money;

    //[SerializeField]
    RawImage[] moneyCamma;

    //[SerializeField]
    RectTransform moneyUiRoot;

    [Space(35)]
    public GameScene_UI_CTRLUI ctrler;
    [SerializeField,Space(5)]
    public CTRLUI_Discription Ctrl_Discription;

    CTRLUI_Discription_TEXTUI ps4, ps5, xbox, keyboard;

    void Start()
    {
        ui = this;

        HP = GameObject.Find("Canvas/BG_HP&MONEY/HPbar").GetComponent<Slider>();
        hpImg = GameObject.Find("Canvas/BG_HP&MONEY/HPbar/Fill Area/Fill").GetComponent<Image>();


        moneyUiRoot = GameObject.Find("Canvas/BG_HP&MONEY/Money").GetComponent<RectTransform>();
        money = new RawImage[12];
        for (int x = 0; x < 12; x++)
            money[x] = GameObject.Find("Canvas/BG_HP&MONEY/Money/" + x).GetComponent<RawImage>();

        moneyCamma = new RawImage[3];
        for (int x = 0; x < 3; x++)
            moneyCamma[x] = GameObject.Find("Canvas/BG_HP&MONEY/Money/camma" + x).GetComponent<RawImage>();

        var c = GameObject.Find("Canvas/Discription").transform;
        ps4 = new CTRLUI_Discription_TEXTUI(c.Find("ps4").gameObject);
        ps5 = new CTRLUI_Discription_TEXTUI(c.Find("ps5").gameObject);
        xbox = new CTRLUI_Discription_TEXTUI(c.Find("xbox").gameObject);
        keyboard = new CTRLUI_Discription_TEXTUI(c.Find("keyboard").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //HP Slider -------------------------------------
        HP.value = HpValue;
        hpImg.color = hpColor.Evaluate(HpValue);

        //Money UI ---------------------------------------
        moneyCamma[2].gameObject.active = MoneyValue > 999999999;
        moneyCamma[1].gameObject.active = MoneyValue > 999999;
        moneyCamma[0].gameObject.active = MoneyValue > 999;
        money[00].enabled = MoneyValue >= 000000000000;
        money[01].enabled = MoneyValue >= 000000000010;
        money[02].enabled = MoneyValue >= 000000000100;
        money[03].enabled = MoneyValue >= 000000001000;
        money[04].enabled = MoneyValue >= 000000010000;
        money[05].enabled = MoneyValue >= 000000100000;
        money[06].enabled = MoneyValue >= 000001000000;
        money[07].enabled = MoneyValue >= 000010000000;
        money[08].enabled = MoneyValue >= 000100000000;
        money[09].enabled = MoneyValue >= 001000000000;
        money[10].enabled = MoneyValue >= 010000000000;
        money[11].enabled = MoneyValue >= 100000000000;
        money[00].texture = num[(int)(MoneyValue % 0000000000010 / 000000000001 % 10)];
        money[01].texture = num[(int)(MoneyValue % 0000000000100 / 000000000010 % 10)];
        money[02].texture = num[(int)(MoneyValue % 0000000001000 / 000000000100 % 10)];
        money[03].texture = num[(int)(MoneyValue % 0000000010000 / 000000001000 % 10)];
        money[04].texture = num[(int)(MoneyValue % 0000000100000 / 000000010000 % 10)];
        money[05].texture = num[(int)(MoneyValue % 0000001000000 / 000000100000 % 10)];
        money[06].texture = num[(int)(MoneyValue % 0000010000000 / 000001000000 % 10)];
        money[07].texture = num[(int)(MoneyValue % 0000100000000 / 000010000000 % 10)];
        money[08].texture = num[(int)(MoneyValue % 0001000000000 / 000100000000 % 10)];
        money[09].texture = num[(int)(MoneyValue % 0010000000000 / 001000000000 % 10)];
        money[10].texture = num[(int)(MoneyValue % 0100000000000 / 010000000000 % 10)];
        money[11].texture = num[(int)(MoneyValue % 1000000000000 / 100000000000 % 10)];
        var c = MoneyUiColor.Evaluate(1f * moneyValue / 999999999999); 
        foreach (var e in money) e.color = c;
        foreach (var e in moneyCamma) e.color = c;
        int digit = moneyCamma[2].gameObject.active ? 3 : (moneyCamma[1].gameObject.active ? 2 : (moneyCamma[0].gameObject.active ? 1 : 0));
        for (var i = moneyValue; i >= 10; i /= 10) digit++;
        moneyUiRoot.localPosition = new Vector3( digit * 6.1f + -166.2f, -7.51f, 0);
        //Ctrler
        ps4?.Update(Ctrl_Discription);
        ps5?.Update(Ctrl_Discription);
        xbox?.Update(Ctrl_Discription);
        keyboard?.Update(Ctrl_Discription);

        if (ps4 != null) ps4.root.active = false;
        if (ps5 != null) ps5.root.active = false;
        if (xbox != null) xbox.root.active = false;
        if (keyboard != null) keyboard.root.active = false;

        switch (ctrler)
        {
            case GameScene_UI_CTRLUI.PS4: ps4.root.active = true; break;
            case GameScene_UI_CTRLUI.PS5: ps5.root.active = true; break;
            case GameScene_UI_CTRLUI.Xbox: xbox.root.active = true; break;
            case GameScene_UI_CTRLUI.KEYBOARD: keyboard.root.active = true; break;
        }
    }
}

public enum GameScene_UI_CTRLUI { Xbox, KEYBOARD, PS4, PS5 }

[System.Serializable]
public class CTRLUI_Discription
{
    [SerializeField]
    public string A, B, X, Y, L, L2, R, R2;
    public string L_JoyStick, R_JoyStick;
}


[System.Serializable]
public class CTRLUI_Discription_TEXTUI
{
    public Text A, B, X, Y, L, L2, R, R2, L_JoyStick, R_JoyStick;
    public GameObject root;

    public CTRLUI_Discription_TEXTUI(GameObject f)
    {
        root = f; 
        A  = f.transform.Find("A").GetComponent<Text>();
        B  = f.transform.Find("B").GetComponent<Text>();
        X  = f.transform.Find("X").GetComponent<Text>();
        Y  = f.transform.Find("Y").GetComponent<Text>();
        L  = f.transform.Find("L").GetComponent<Text>();
        R  = f.transform.Find("R").GetComponent<Text>();
        L2 = f.transform.Find("L2").GetComponent<Text>();
        R2 = f.transform.Find("R2").GetComponent<Text>();
        L_JoyStick = f.transform.Find("JSL").GetComponent<Text>();
        R_JoyStick = f.transform.Find("JSR").GetComponent<Text>();
    }

    public void Update(CTRLUI_Discription c, bool f = false)
    {
        root.active = f;
        A.text = c.A;
        B.text = c.B;
        X.text = c.X;
        Y.text = c.Y;
        L.text = c.L;
        L2.text = c.L2;
        R.text = c.R;
        R2.text = c.R2;
        L_JoyStick.text = c.L_JoyStick;
        R_JoyStick.text = c.R_JoyStick;
    }
}