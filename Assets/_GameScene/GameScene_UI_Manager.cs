using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene_UI_Manager : MonoBehaviour
{
    public static GameScene_UI_Manager ui;
    public static bool FocusChatTextBox;
    public Texture2D TeamIcon_Resaler, TeamIcon_Police; 
    [Space(10), SerializeField,Range(0,1f)] float HpValue; 
    [SerializeField] Gradient hpColor;  
    [SerializeField, Range(0,99999999),Space(15)] long MoneyValue; 
    [SerializeField] Gradient MoneyUiColor; 
    [SerializeField] Texture2D[] num; 
    [Space(35)] public GameScene_UI_CTRLUI ctrler; 
    [SerializeField,Space(25)] public CTRLUI_Discription Ctrl_Discription; 
    [Space(55), SerializeField] public WeaponUIData Weapon0;
    [Space(15), SerializeField] public WeaponUIData Weapon1;
    


    Slider HP;
    WeaponUI_obj WeaponUI0, WeaponUI1;
    RawImage[] money;
    Image hpImg;
    RawImage[] moneyCamma;
    RectTransform moneyUiRoot;
    GameObject chatBOX, ctrlerDiscription;
    CTRLUI_Discription_TEXTUI ps4, ps5, xbox, keyboard;
    public RawImage TeamICON;
    TimerUI timeCount;
    GameSceneSystem mainSys;


    public Texture2D[] getNum
    {
        get
        {
            return num;
        }
    }

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



    void Start()
    {
        mainSys = GetComponent<GameSceneSystem>();
        ui = this; 
        TeamICON = GameObject.Find("Canvas/teams").GetComponent<RawImage>();
        timeCount = GameObject.Find("TimerUI_Canvas/TimerUI").GetComponent<TimerUI>();
        //チームアイコンの割り振り
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
        chatBOX = GameObject.Find("Canvas/Chat");
        ctrlerDiscription = GameObject.Find("Canvas/Discription");
        int gsui = PlayerPrefs.GetInt("GS-UI", 3);
        chatBOX.active = gsui /2 == 1;
        ctrlerDiscription.active = gsui % 2 == 1;
        WeaponUI0 = new WeaponUI_obj(GameObject.Find("Canvas/WeaponUI1"));
        WeaponUI1 = new WeaponUI_obj(GameObject.Find("Canvas/WeaponUI1"));
        chatBOX.active = false;
        ctrlerDiscription.active = false;
    }

    void OnDestroy()
    {
        SaveUIEbl();
    }



    void Update()
    {
        //時間の更新------------------------------------------------
        timeCount.time = mainSys.time;

        //Input-------------------------------------------
        if (Key.FL.Down) chatBOX.active = !chatBOX.active;
        if (Key.FR.Down) ctrlerDiscription.active = !ctrlerDiscription.active;

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
        //Timer

        //Team ICON

        //Weapon
        WeaponUI0.Update(Weapon0);
        WeaponUI1.Update(Weapon1);

    }




    void SaveUIEbl()
    {
       // PlayerPrefs.SetInt("GS-UI", (chatBOX.active ? 2 : 0) + (ctrlerDiscription.active ? 1 : 0));
        PlayerPrefs.Save();
    }



}








/// <summary>
/// 右下の操作説明UIにおいてどれを表示するか?
/// </summary>
public enum GameScene_UI_CTRLUI
{
    Xbox,
    KEYBOARD,
    PS4,
    PS5
}




/// <summary>
/// 右下の操作説明の説明文のデータ
/// </summary>
[System.Serializable]
public class CTRLUI_Discription
{

    /// <summary>
    /// Aキーの操作説明文
    /// </summary>
    [SerializeField]
    public string A;



    /// <summary>
    /// Bキーの操作説明文
    /// </summary>
    [SerializeField]
    public string B;



    /// <summary>
    /// Xキーの操作説明文
    /// </summary>
    [SerializeField]
    public string X;



    /// <summary>
    /// Yキーの操作説明文
    /// </summary>
    [SerializeField]
    public string Y;



    /// <summary>
    /// 左トリガーの操作説明文
    /// </summary>
    [SerializeField]
    public string L;



    /// <summary>
    /// 左縦トリガーの操作説明文
    /// </summary>
    [SerializeField]
    public string L2;



    /// <summary>
    /// 右トリガーの操作説明文
    /// </summary>
    [SerializeField]
    public string R;



    /// <summary>
    /// 右縦トリガーの操作説明文
    /// </summary>
    [SerializeField]
    public string R2;



    /// <summary>
    /// 左ジョイスティックの操作説明文
    /// </summary>
    [SerializeField]
    public string L_JoyStick;



    /// <summary>
    /// 右ジョイスティックの操作説明文
    /// </summary>
    [SerializeField]
    public string R_JoyStick;

}






/// <summary>
/// 画面右下の操作説明UIを管理するクラス
/// </summary>
[System.Serializable]
public class CTRLUI_Discription_TEXTUI
{
    /// <summary>
    /// AキーのUI
    /// </summary>
    public Text A;


    /// <summary>
    /// BキーのUI
    /// </summary>
    public Text B;


    /// <summary>
    /// XキーのUI
    /// </summary>
    public Text X;


    /// <summary>
    /// YキーのUI
    /// </summary>
    public Text Y;


    /// <summary>
    /// LキーのUI
    /// </summary>
    public Text L;


    /// <summary>
    /// LTキーのUI
    /// </summary>
    public Text L2;


    /// <summary>
    /// RキーのUI
    /// </summary>
    public Text R;


    /// <summary>
    /// RTキーのUI
    /// </summary>
    public Text R2;


    /// <summary>
    /// 左ジョイスティックのUI
    /// </summary>
    public Text L_JoyStick;

    /// <summary>
    /// 右ジョイスティックのUI
    /// </summary>
    public Text R_JoyStick;


    /// <summary>
    /// 親オブジェクト
    /// </summary>
    public GameObject root;



    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="f"></param>
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



    /// <summary>
    /// 更新する
    /// </summary>
    /// <param name="c">表示情報</param>
    /// <param name="f">UIを描画するか(def = false)</param>
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






/// <summary>
/// 武器情報のデータ
/// </summary>
[System.Serializable]
public class WeaponUIData
{
    /// <summary>
    /// 表示するか?
    /// </summary>
    public bool enable;

    /// <summary>
    /// 武器のアイコン
    /// </summary>
    public Texture2D icon;
    
    
    /// <summary>
    /// 武器名
    /// </summary>
    public string weaponName;

    /// <summary>
    /// 残弾数
    /// </summary>
    [Range(0, 999)]
    public int now;

    /// <summary>
    /// 最大弾数
    /// </summary>
    [Range(0,999)]
    public int max;

    /// <summary>
    /// 武器番号
    /// </summary>
    [Range(0,1)]
    public int weaponNumber;

    /// <summary>
    /// リロード状態
    /// <para>(0.0f～1.0f)</para>
    /// </summary>
    [Range(0,1f)]
    public float reload;

}




/// <summary>
/// 武器情報を表示するUIのオブジェクトデータ
/// </summary>
public class WeaponUI_obj
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="object">親オブジェクト</param>
    public WeaponUI_obj(GameObject @object)
    {
        root = @object;
        Icon = @object.transform.Find("root/Icon").GetComponent<RawImage>();
        Name = @object.transform.Find("root/Name").GetComponent<Text>();
        WeaponNumber = @object.transform.Find("root/WeaponNumber").GetComponent<Text>();
        Reload = @object.transform.Find("root/Reload").GetComponent<Slider>();
        n2 = @object.transform.Find("root/x00-000").GetComponent<RawImage>();
        n1 = @object.transform.Find("root/0x0-000").GetComponent<RawImage>();
        n0 = @object.transform.Find("root/00x-000").GetComponent<RawImage>();
        m2 = @object.transform.Find("root/000-x00").GetComponent<RawImage>();
        m1 = @object.transform.Find("root/000-0x0").GetComponent<RawImage>();
        m0 = @object.transform.Find("root/000-00x").GetComponent<RawImage>();
    }


    /// <summary>
    /// UIを更新
    /// </summary>
    /// <param name="data">情報(WeaponUIData型)</param>
    public void Update(WeaponUIData data)
    {
        root.active = data.enable;
      //  Icon.texture = data.icon;
      //  Reload.value = data.reload;
        WeaponNumber.text = "Weapon" + data.weaponNumber;
        Name.text = data.weaponName;
        int now = data.now < 0 ? 0 : (data.now > 999 ? 999 : data.now);
        int max = data.max < 0 ? 0 : (data.max > 999 ? 999: data.max);

        n2.texture = GameScene_UI_Manager.ui.getNum[now / 100];
        n2.texture = GameScene_UI_Manager.ui.getNum[now % 100/10];
        n2.texture = GameScene_UI_Manager.ui.getNum[now % 10];

        m2.texture = GameScene_UI_Manager.ui.getNum[max / 100];
        m2.texture = GameScene_UI_Manager.ui.getNum[max % 100 / 10];
        m2.texture = GameScene_UI_Manager.ui.getNum[max % 10];
    }

    /// <summary>
    /// 武器のアイコン
    /// </summary>
    public RawImage Icon;

    /// <summary>
    /// 武器名
    /// </summary>
    public Text Name;

    /// <summary>
    /// 武器番号を表示するtext(Weapon?の表記)
    /// </summary>
    public Text WeaponNumber;

    /// <summary>
    /// リロードのバー
    /// </summary>
    public Slider Reload;

    /// <summary>
    /// 残弾数のUI
    /// </summary>
    public RawImage n2, n1, n0;

    /// <summary>
    /// 残弾数のUI
    /// </summary>
    public RawImage m2, m1, m0;

    /// <summary>
    /// ルートオブジェクト
    /// </summary>
    public GameObject root;


}






 


 


