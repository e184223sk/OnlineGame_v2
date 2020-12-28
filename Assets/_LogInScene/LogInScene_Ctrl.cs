using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
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
        BadAccount;



    void Start()
    {
        Cursor.visible = true;
        ButtonEvent_SignUp_Return();
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
        if(EXISTURL(SignIn_UserName.text + @"\exist.tagtx"))
        {
            string PIN = Random.Range(10201020, 99999999).ToString().PadLeft(8,'1');
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
            "　　P.I.N       :" + PIN.Substring(0,4) + "-" + PIN.Substring(4) + "\n" +
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
            ButtonEvent_SignUp_Return();
            SendText.active = true;

            string user = SignUp_UserName.text;
            string pin_ = PIN; 


            string userDir = SignIn_UserName.text;
            string passDir = SignIn_UserName.text + @"\" + SignIn_Pin.text;
            string chatDir = passDir + @"\chat";

            string userEXIST = userDir + @"\exist.tagtx";
            string pin_EXIST   = passDir + @"\exist.tagtx";
            string data_config = passDir + @"\config.txt";
            string data_avator = passDir + @"\avator.txt";
            string data_log    = passDir + @"\log.txt"; 

             

        }
    }


    public void ButtonEvent_SignIn_LogIn()
    {
        Update();
        if (EXISTURL(SignIn_UserName.text + @"\" + SignIn_Pin.text+@"\exist.tagtx"))
        {
            NetData.user = new USERDATA(SignIn_UserName.text, SignIn_Pin.text);
            Cursor.visible = false;
            SceneLoader.LoadN("LogoScene");
        }
    }



    bool EXISTURL(string x)
    { 
        string c = NetData.ServerRoot + @"\" + NetData.ServerUser + @"\";
        var req = UnityWebRequest.Get(c + x);
        var v = req.responseCode;
        req.Dispose();
        return v < 400;
    }
     
}
 
