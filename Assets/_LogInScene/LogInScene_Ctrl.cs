using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogInScene_Ctrl : MonoBehaviour
{
    [SerializeField]
    InputField 
        SignIn_UserName,
        SignIn_Pin,
        SignUp_UserName,
        SignUp_MAIL;

    [SerializeField]
    GameObject
        SignUpRoot,
        SignInRoot,
        SendText,
        BadAccount,
        exist_Comment;



    void Start()
    {
        Cursor.visible = true;
        ButtonEvent_SignUp_Return();

        exist_Comment.active = false;
    }

    void Update()
    {
        SignIn_UserName.text = SignIn_UserName.text.Replace(" ", "").Replace("　", "");
        SignIn_Pin.text = SignIn_Pin.text.Replace(" ", "").Replace("　", "");
        SignUp_UserName.text = SignUp_UserName.text.Replace(" ", "").Replace("　", "");
        SignUp_MAIL.text = SignUp_MAIL.text.Replace(" ", "").Replace("　", "");
       
    }


    public void ButtonEvent_SignUp_Return()
    {
        Update();
        SignUp_UserName.text = "";
        SignUp_MAIL.text = "";
        SignUpRoot.active = false;
        SignInRoot.active =true;
    }


    public void ButtonEvent_SignIn_SignUp()
    {
        Update();
        SignUpRoot.active = true;
        SignInRoot.active = false;
    }
     
    public void ButtonEvent_SignUp_Send()
    {
        Update();

        SetDATAS(SignUp_UserName.text, "");
        bool k = EXISTURL(NetData.userEXIST);

        exist_Comment.active = k;
        if (!k)
        {
            string PIN = Random.Range(10201020, 99999999).ToString().PadLeft(8, '1');

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(NetData.MailAddress, "Picture (久留米工業大学)");
            msg.To.Add(new MailAddress(SignUp_MAIL.text, SignUp_UserName.text));
            msg.Subject = "ようこそ!! 「ゲーム名(仮)」へ";
            msg.Body =
            "ようこそ!! " + SignUp_UserName.text + "様!!\n\n" +
            " 初めまして!! Picture です!!  " + "\n" +
            "この度はサインアップいただきありがとうございます。" + "\n" +
            "本ゲームには課金等はございません。思う存分お楽しみください" + "\n" +
            " " + "\n" +
            " " + "\n" +
            "--------------------------------------------------------" + "\n" +
            "　　アカウント名:" + SignUp_UserName.text + "\n" +
            " " + "\n" +
            "　　P.I.N       :" + PIN.Substring(0, 4) + "-" + PIN.Substring(4) + "\n" +
            "--------------------------------------------------------" + "\n" +
            " " + "\n" +
            " もし心当りがなければ下記連絡先までご連絡ください。" + "\n" +
            " " + "\n" +
            "◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆" + "\n" +
            " PICTURE (久留米工業大学 学生団体)" + "\n" +
            " mail :: " + NetData.MailAddress + "\n" +
            " URL :: " + @"https://pictureanimationst9.wixsite.com/mysite" + " \n" +
            " " + "\n" +
            " ※メールの返信は翌日(土日,祝日,年末年始を除く)となる場合がございます\n" +
            "◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆" + "\n";




            SmtpClient sc = new SmtpClient();
            sc.Host = "smtp.gmail.com";
            sc.Port = 587;
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            sc.Credentials = new System.Net.NetworkCredential(NetData.MailAddress, NetData.MailPASS);
            sc.EnableSsl = true;

            sc.Send(msg);
            msg.Dispose();





            SendText.active = true;

            BadAccount.active = false;

            SetDATAS(SignUp_UserName.text, PIN);

            NetData.server.MakeDirectory(NetData.userDir, null, Debug.Log);
            NetData.server.MakeDirectory(NetData.passDir, null, Debug.Log);
            NetData.server.MakeDirectory(NetData.chatDir, null, Debug.Log);
            NetData.server.MkFile(NetData.userEXIST, null, Debug.Log);
            NetData.server.MkFile(NetData.pin_EXIST);
            NetData.server.MkFile(NetData.data_config);
            NetData.server.MkFile(NetData.data_log);

            ButtonEvent_SignUp_Return();
        } 

    }


    void SetDATAS(string user, string pin)
    {
        NetData.userDir = "OnlineSystem/_DATA/_USER_DATA__/" + user;
        NetData.passDir = NetData.userDir + @"/" + pin ;
        NetData.chatDir = NetData.passDir + @"/chat";

        NetData.userEXIST = NetData.userDir + @"/exist.tagtx";
        NetData.pin_EXIST = NetData.passDir + @"/exist.tagtx";
        NetData.data_config = NetData.passDir + @"/data.txt";
        NetData.data_log = NetData.passDir + @"/log.txt";
        NetData.user = new USERDATA(user, pin);

    }
    public void ButtonEvent_SignIn_LogIn()
    {
        Update();

        SetDATAS(SignIn_UserName.text, SignIn_Pin.text);

        if (EXISTURL(NetData.pin_EXIST))
        {
            Debug.Log("TRue");
            Cursor.visible = false;
            ConfigData_Manager.LOAD();
            LogData_Manager.LOAD();
            SceneLoader.LoadN("LogoScene");
        }
        else
        {
            Debug.Log("false");
            SignIn_UserName.text = SignIn_Pin.text = "";
            SendText.active = false;
            BadAccount.active = true;
        }
    } 

    bool EXISTURL(string x)
    {
        x = "http://xs238699.xsrv.jp/picture/onlineGame_2020/" +  x ;
        return ((int)NetData.server.GetStatusCode(x)) < 400;
    }
     


}

