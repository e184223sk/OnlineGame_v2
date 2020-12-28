using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetData : MonoBehaviour
{
    public const string MailAddress = "";
    public const string MailPASS    = "";
    public const string ServerAddress = "";
    public const string ServerPASS = "";

    public const string ServerRoot  = "";
    public const string ServerUser = @"_DATA\_USER_DATA__\";
    public const string ServerLogs = @"_DATA\_LOG_DATA__\";

    public static USERDATA user = new USERDATA();
    public static USERDATA[] friends;
}


public class USERDATA
{
    public string ID = "", PASS = "";
    public USERDATA() { }
    public USERDATA(string id, string pass) { ID = id; PASS = pass; }
}