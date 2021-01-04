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
        int VersionData = int.Parse(File.ReadAllText(dir + "version.txt"));
        File.WriteAllText(dir + "version.txt", (++VersionData).ToString().PadLeft(8, '0')); 
        Console.WriteLine("make List File");
        string[] files_ = GetAllFiles(dir).ToArray();
        List<string> xdir = new List<string>();
        for (var f = 0; f < files_.Length; f++) files_[f] = files_[f].Substring(dir.Length);
        int vv = 0;
        foreach (var ff in files_)
        {
            Console.WriteLine("GetFILE" + vv.ToString().PadLeft(6, '0') + " :: " + ff);
            vv++;
        }

        foreach (var ff in files_)
        {
            try
            {
                string a = ff;
                do
                {
                    a = a.Substring(0, a.LastIndexOf(@"\"));
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

        //アップロード -----------------------------------------
        PostProcess server = new PostProcess( );
        server.SetRoot("GameSystem/System/");

        Console.WriteLine("make dir in server");
        foreach (var cc in xdir.ToArray())
            server.MakeDirectory(cc);

        Console.WriteLine("upload file");

        int xxx__ = 0;
        foreach (var cc in files_)
        {
            xxx__++;
            server.FileUpload(dir + cc, cc);
        }
        Console.WriteLine("fin");
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

  


public class PostProcess
{ 
    HttpClient _httpClient = null;
    public void SetRoot(string x) { accessUrlBase = "ftp://" + Address + "/%2f" + x; }
    string accessUrlBase = "";
    public const string Address = "csys.00.og20@xs238699.xsrv.jp";
    public const string PASS = "fd2rf4yh";
    public const string Domain = "xs238699.xsrv.jp";

    ~PostProcess() { Dispose(); }

    public PostProcess()
    {

    }



    public void Dispose()
    { 
        _httpClient.Dispose();
    }

    public async void ExecuteHttpClient(string url)
    {
        string imagePath = @"C:\Users\Default\Pictures\white.jpg";

        MultipartFormDataContent content = new MultipartFormDataContent();
        // バイナリデータ.
        ByteArrayContent imageBytes = new ByteArrayContent(File.ReadAllBytes(imagePath));
        content.Add(imageBytes, "imagefile", Path.GetFileName(imagePath));

        // 文字列.
        content.Add(new StringContent("E195335D5206A8CC06312DC2717CB514"), "checkkey");
        content.Add(new StringContent("1"), "id");
        content.Add(new StringContent("sandy"), "name");
        _httpClient.PostAsync(new Uri(accessUrlBase + url), content);
    }



    public void FileUpload(string local, string url)
    {


    }


    public void MakeDirectory(string url)
    {

    }

}