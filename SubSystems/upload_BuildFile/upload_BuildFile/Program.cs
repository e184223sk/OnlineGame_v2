using System;
using System.Collections.Generic;
using System.IO; 
using System.Net; 
using System.Net.NetworkInformation; 
using System.Collections;
using System.Reflection; 
using System.Runtime.InteropServices;
 
[assembly: AssemblyTitle("upload_BuildFile")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Picture")]
[assembly: AssemblyProduct("upload_BuildFile")]
[assembly: AssemblyCopyright("Copyright © Picture 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("4a560e63-d4d8-42ac-b6df-e495290e17fa")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


class SORT : IComparer
{
    public int Compare(object x, object y)
    { 
        return ((string)x).Length - ((string)y).Length;
    }
}

class Program
{

    static void Main(string[] args)
    {
        Console.WindowWidth = Console.WindowWidth/2;
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
        dir = Directory.GetParent(Directory.GetParent(dir).ToString()).ToString() + @"\Build\System\";
        int VersionData = int.Parse(File.ReadAllText(dir + "version.txt"));
        File.WriteAllText(dir + "version.txt", (++VersionData).ToString().PadLeft(8, '0'));

        Console.WriteLine("local root directory (buildroot): " + buildroot);

        //リスト作成

        Console.WriteLine("make List File");
        string[] files_ = GetAllFiles(dir).ToArray();
        List<string> xdir = new List<string>();
        for (var f = 0; f < files_.Length; f++)
            files_[f] = files_[f].Substring(dir.Length);
        int vv = 0;
        foreach (var ff in files_)
        {
            Console.WriteLine("GetFILE" +vv.ToString().PadLeft(6,'0')+ " :: " + ff);
            vv++;
        }
        ///フォルダ配列の作成
        foreach (var ff in files_)
        {
            try
            {
                string a = ff;
                do
                {
                    a = a.Substring(0,a.LastIndexOf(@"\"));
                    if (!xdir.Contains(a))
                        xdir.Add(a);
                    Console.WriteLine("BackDIR:" + a);
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
        File.WriteAllLines( buildroot + @"\Build\gameL.txt", files_);

        Console.WriteLine("made List File");
        //アップロード -----------------------------------------

        Console.WriteLine("start conecting server ");
        Server server = new Server(NetData.ServerAddress, NetData.ServerPASS, NetData.ServerDomain);
        server.SetRoot("GameSystem/System/");

        Console.WriteLine("finished conecting server ");

        Console.WriteLine("start upload");


        Console.WriteLine("make dir in server");
        foreach (var cc in xdir.ToArray())
            server.MakeDirectory( cc, delegate() { Console.WriteLine("fin_mkdir:" + cc); }, delegate(string x ) { Console.WriteLine("err_mkdir:" + cc + ":" + x); });

        Console.WriteLine("upload file");

        int xxx__ = 0;
        foreach (var cc in files_)
        { 
            xxx__++;
            
            if (cc == "update.exe")
                Console.WriteLine("skip_upload(" + xxx__ + "/" + files_.Length + ") : update.exe(不要なファイルのためスキップされました)");
            else
                server.FileUpload(dir + cc, cc, delegate () { Console.WriteLine("fin_upload(" + xxx__ + "/" + files_.Length + ") : " + cc); }, Console.WriteLine);

        }

        Console.WriteLine("start baseSystem "); 
        server.SetRoot("GameSystem/GameRoot/");
        var GRF = File.ReadAllLines(buildroot + @"\Build\{uploadSubFileList}.txt");
        server.FileUpload(buildroot + @"\Build\roots\start.exe", "start.exe", delegate () { Console.WriteLine("fin_uploadLIST"); }, Console.WriteLine);//リストアップロード
        server.FileUpload(buildroot + @"\Build\roots\update.exe    ", "update.exe", delegate () { Console.WriteLine("fin_uploadLIST"); }, Console.WriteLine);//リストアップロード

        server.MakeDirectory(buildroot + @"\Build\subFile\System");

        foreach (var c in GRF)
        {
            string cc = buildroot + @"\Build\subFile\" + c;

            if (File.Exists(cc))
            {
                Console.WriteLine("upload GameSystems File::" + c + "(" + cc + ")");
                server.FileUpload(cc, c.ToString().Replace(@"\", "/"), delegate () { Console.WriteLine("fin_uploadLIST"); }, Console.WriteLine);//リストアップロード
            }
            else
                result += " Not Exist:" + cc + "\n";
        }
          
        Console.WriteLine("start baseSystem ");
        server.SetRoot("GameSystem/");

        server.FileUpload(buildroot + @"\Build\gameL.txt", "SystemList.txt", delegate () { Console.WriteLine("fin_uploadLIST"); }, Console.WriteLine);//リストアップロード
        server.FileUpload(buildroot + @"\Build\{uploadSubFileList}.txt", "GameRootList.txt", delegate () { Console.WriteLine("fin_uploadLIST"); }, Console.WriteLine);//リストアップロード
        Console.WriteLine("fin");

        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        Console.WriteLine("RESULT >>> ");
        Console.WriteLine(result);
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
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
 
public class NetData 
{
    public const string ServerAddress = "csys.00.og20@xs238699.xsrv.jp";
    public const string ServerPASS = "fd2rf4yh";
    public const string ServerDomain = "xs238699.xsrv.jp"; 
}
//ftp://" + useraddress/%2f
 
public class Server
{
    //デリゲートの型宣言-----------------------------------
    public delegate void TaskEvent();
    public delegate void ErrorEvent(string text);


    //パラメータ関連 --------------------------------------
    public string useraddress;
    public string password;
    public string serveraddress;
    public void SetRoot(string x)  { accessUrlBase = "ftp://" + useraddress + "/%2f" + x; }
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
        Dispose();
    }

    public void Dispose()
    {
        webClient?.Dispose();
        icr = null;
        useraddress = null;
        password = null;
        serveraddress = null;
        accessUrlBase = null;
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

