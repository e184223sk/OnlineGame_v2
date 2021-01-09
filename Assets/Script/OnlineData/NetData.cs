using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetData : MonoBehaviour
{
    public const string MailAddress = "picture.kit.kurume@gmail.com";
    public const string MailPASS    = "picpicpicture1616";
    public const string ServerAddress = "csys.00.og20@xs238699.xsrv.jp";
    public const string ServerPASS = "fd2rf4yh";
    public const string ServerDomain  = "xs238699.xsrv.jp";
    public const string ServerRoot  = "OnlineSystem";
    public const string LOGDIR = "OnlineSystem/_DATA/_LOG_DATA__/";
    public const string USERDIR = "OnlineSystem/_DATA/_USER_DATA__/";

    public static USERDATA user = new USERDATA();
    public static USERDATA[] friends;
    public static Server server = new Server(ServerAddress, ServerPASS, ServerDomain);
    public static string userDir, passDir, chatDir, userEXIST, pin_EXIST, data_config, data_log;


    public static void SetDATAS(string user, string pin)
    {
        NetData.userDir = "OnlineSystem/_DATA/_USER_DATA__/" + user;
        NetData.passDir = NetData.userDir + @"/" + pin;
        NetData.chatDir = NetData.passDir + @"/chat";

        NetData.userEXIST = NetData.userDir + @"/exist.tagtx";
        NetData.pin_EXIST = NetData.passDir + @"/exist.tagtx";
        NetData.data_config = NetData.passDir + @"/data.txt";
        NetData.data_log = NetData.passDir + @"/log.txt";
        NetData.user = new USERDATA(user, pin);

    }



    public static string Get__USERSPACE
    {
        get => USERDIR + user.ID + "/" + user.PASS + "/";
    }

    public static string Get__DATA_TXT { get => Get__USERSPACE + "data.txt"; }
    public static string Get__LOG_TXT { get => Get__USERSPACE + "data.txt"; }
    public static string Get__CHATDIR { get => Get__USERSPACE + "chat/"; }

}


public class USERDATA
{
    public string ID = "", PASS = "";
    public USERDATA() { }
    public USERDATA(string id, string pass) { ID = id; PASS = pass; }
}


//http://xs238699.xsrv.jp/picture/onlineGame_2020/

//ftp://csys.00.og20@xs238699.xsrv.jp/%2fxs238699.xsrv.jp/public_html/

//http://xs238699.xsrv.jp/picture/onlineGame_2020/OnlineSystem