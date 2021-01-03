using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;

[assembly: AssemblyTitle("start")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Picture")]
[assembly: AssemblyProduct("start")]
[assembly: AssemblyCopyright("Copyright © Picture 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")] 
[assembly: ComVisible(false)] 
[assembly: Guid("082809f8-cdca-4810-8356-471b7a1e8760")] 
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

class Program
{
    static void Main(string[] args)
    {
        string url = "http://xs238699.xsrv.jp/picture/onlineGame_2020/GameSystem/System/version.txt";
        string root = AppDomain.CurrentDomain.BaseDirectory + @"System\";
        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) ERROR("ネットワークに接続されていません");
        if (!File.Exists(root + "OnlineGame.exe")) ERROR("システムが正常に機能していません。再インストールしてください(ErrorCode:0x00)");
        if (!File.Exists(root + "update.exe"    )) ERROR("システムが正常に機能していません。再インストールしてください(ErrorCode:0x01)");
        if (!File.Exists(root + "version.txt"   )) ERROR("システムが正常に機能していません。再インストールしてください(ErrorCode:0x02)");
        Process.Start(root + ((File.ReadAllText(root + @"version.txt") != new WebClient().DownloadString(url)) ? "update.exe" : "OnlineGame.exe"));
    }

    static void ERROR(string text)
    {
        MessageBox.Show(text, "ERROR!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Environment.Exit(1);
    }
}
 