using UnityEngine;

public class GameSceneUIController : MonoBehaviour
{

    public static float HP
    {
        set => GameScene_UI_Manager.ui.hpValue = value > 1 ? 1 : (value < 0 ? 0 : value);
        get => GameScene_UI_Manager.ui.hpValue;
    }

    
    public static long Money
    {
        get => GameScene_UI_Manager.ui.moneyValue;
        set => GameScene_UI_Manager.ui.moneyValue = value > 999999999999 ? 999999999999 : (value < 0 ? 0 : value);
    }


    public static void SetTeamIcon_Resealer() => GameScene_UI_Manager.ui.TeamICON.texture = GameScene_UI_Manager.ui.TeamIcon_Resaler;
    public static void SetTeamIcon_Police  () => GameScene_UI_Manager.ui.TeamICON.texture = GameScene_UI_Manager.ui.TeamIcon_Police;

    public WeaponUIData Weapon0
    {
        get => GameScene_UI_Manager.ui.Weapon0;
        set => GameScene_UI_Manager.ui.Weapon0 = value;
    }

    public WeaponUIData Weapon1
    {
        get => GameScene_UI_Manager.ui.Weapon1;
        set => GameScene_UI_Manager.ui.Weapon1 = value;
    }

    public string DiscriptionText_A
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.A;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.A = value;
    }

    public string DiscriptionText_B
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.B;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.B = value;
    }

    public string DiscriptionText_X
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.X;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.X = value;
    }

    public string DiscriptionText_Y
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.Y;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.Y = value;
    }


    public string DiscriptionText_L
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.L;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.L = value;
    }


    public string DiscriptionText_L2
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.L2;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.L2 = value;
    }


    public string DiscriptionText_R
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.R;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.R = value;
    }


    public string DiscriptionText_R2
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.R2;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.R2 = value;
    }

    public string DiscriptionText_JoyStickL
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.L_JoyStick;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.L_JoyStick = value;
    }


    public string DiscriptionText_JoyStickR
    {
        get => GameScene_UI_Manager.ui.Ctrl_Discription.R_JoyStick;
        set => GameScene_UI_Manager.ui.Ctrl_Discription.R_JoyStick = value;
    }


}
