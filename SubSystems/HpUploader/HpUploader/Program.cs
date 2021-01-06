using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

using System;
using System.IO;
using System.Net.Http;

class Program
{
    static void Main(string[] args)
    {
        Console.WindowWidth /= 2;
        string result = "";
        Console.WriteLine("Check NetWork");

        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            Console.WriteLine("ネットワークに接続されていません");
            Console.Read();
            Environment.Exit(1);
        }

        Console.WriteLine("ネット接続確認\n\n\n");
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        string buildroot = Directory.GetParent(Directory.GetParent(dir).ToString()).ToString();
        dir = Directory.GetParent(Directory.GetParent(dir).ToString()).ToString() + @"\Hp\";

        Console.WriteLine("make List");
        string[] files_ = GetAllFiles(dir).ToArray();
        List<string> xdir = new List<string>();

        for (var f = 0; f < files_.Length; f++)
            files_[f] = files_[f].Substring(dir.Length);
        
        for (int v = 0; v < files_.Length; v++)
            Console.WriteLine("GetFILE" + v.ToString().PadLeft(6, '0') + " :: " + files_[v]);

        foreach (var ff in files_)
        {
            try
            {
                string a = ff;
                do
                {
                    a = a.Substring(0, a.LastIndexOf(@"\"));
                    if (!xdir.Contains(a)) xdir.Add(a); 
                }
                while (a != "");
            }
            catch (Exception) { }
        }

        for (int ff = 0; ff < xdir.Count; ff++) xdir[ff] = xdir[ff].Replace(@"\", "/");
        string[] fc = xdir.ToArray();
        Array.Sort(fc, new SORT());
        xdir = new List<string>(fc);
        foreach (var ff in xdir.ToArray()) Console.WriteLine("Dir :: " + ff);

        Server server = new Server(NetData.ServerAddress, NetData.ServerPASS, NetData.ServerDomain);
        server.SetRoot(NetData.ROOT);

        foreach (var cc in xdir.ToArray()) 
            server.MakeDirectory(cc, delegate () { Console.WriteLine("mkdir::" + cc); }, delegate (string x) { if(!x.Contains("Exist")) Console.WriteLine("err::" + cc); });
         
        for (int f = 0; f < files_.Length; f++) 
            server.FileUpload(dir + files_[f], files_[f], delegate() { Console.WriteLine("fin::" + files_[f]); }, delegate (string x) { Console.WriteLine("err::" + files_[f]); });
        
        Console.WriteLine("アップロードが完了しました");
        Console.ReadLine();
    }
     

    public static List<String> GetAllFiles(String DirPath)
    {
        List<String> lstStr = new List<String>();    // 取得したファイル名を格納するためのリスト
        String[] strBuff;   // ファイル名とディレクトリ名取得用

        try
        {
            // ファイル名取得
            strBuff = Directory.GetFiles(DirPath, "*");        // 探索範囲がルートフォルダで時間が掛かるため、テキスト形式のファイルのみ探索
            foreach (string file in strBuff) lstStr.Add(file);

            strBuff = Directory.GetDirectories(DirPath);
            foreach (string directory in strBuff)
            {
                List<string> lstBuff = GetAllFiles(directory);    // 取得したディレクトリ名を引数にして再帰
                lstBuff.ForEach(delegate (string str) { lstStr.Add(str); });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return lstStr;

    }

}


class SORT : IComparer
{
    public int Compare(object x, object y)
    {
        return ((string)x).Length - ((string)y).Length;
    }
}






public class Server
{
    //デリゲートの型宣言-----------------------------------
    public delegate void TaskEvent();
    public delegate void ErrorEvent(string text);


    //パラメータ関連 --------------------------------------
    public string useraddress;
    public string password;
    public string serveraddress;
    public void SetRoot(string x) { accessUrlBase = "ftp://" + useraddress + "/%2f" + x; }
    string accessUrlBase = "";
    private WebClient webClient;
    private ICredentials icr;

    //コンストラクタ -------------------------------------
    public Server(string useraddress, string password, string serveraddress)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        this.useraddress = useraddress;
        this.password = password;
        this.serveraddress = serveraddress;
        this.accessUrlBase = "ftp://" + useraddress + "/%2f";
        icr = new NetworkCredential(useraddress, password);
        webClient = new WebClient() { Credentials = new NetworkCredential(useraddress, password) };
    }

    //デストラクタ関連  ----------------------------------
    ~Server()
    {
        webClient?.Dispose();
        icr = null; 
    }
     


    public void FileUpload(string local, string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            webClient.UploadFile(accessUrlBase + url, local);
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }


    public void MakeDirectory(string url, TaskEvent finished = null, ErrorEvent error = null)
    {
        try
        {
            //Console.WriteLine("uri full path:" + accessUrlBase + url);
            FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(accessUrlBase + url);
            ftpReq.Credentials = icr;
            ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse();
            ftpRes.Close();
            finished?.Invoke();
        }
        catch (WebException e)
        {
            error?.Invoke(((FtpWebResponse)e.Response).StatusDescription);
        }
        catch (Exception e)
        {
            error?.Invoke(e.Message);
        }
    }





}




public class NetData
{
    public const string ServerAddress = "csys.hp.og20@xs238699.xsrv.jp";
    public const string ServerPASS = "ffiwoefks";
    public const string ServerDomain = "xs238699.xsrv.jp";
    public const string ROOT = "game20202021/";
}