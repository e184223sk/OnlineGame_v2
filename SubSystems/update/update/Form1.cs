using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace update
{
    public partial class Form1 : Form
    { 
        string root = "http://xs238699.xsrv.jp/picture/onlineGame_2020/GameSystem/System/";
        string[] fileList;
        int cnt;
        string localDL;
        void CC(string x) { File.WriteAllText(@"C:\Users\yuitiro\Music\cc\ppp.txt", x); }
        Timer t,t2;
        bool isEnd;

        public Form1()
        {  
            InitializeComponent();
            var ListGet = new WebClient();
            fileList = ListGet.DownloadString("http://xs238699.xsrv.jp/picture/onlineGame_2020/GameSystem/SystemList.txt").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            ListGet.Dispose();
            localDL = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + @"\";
            foreach (var i in Path.GetInvalidPathChars())
                localDL = localDL.Replace(i + "", "");

            //F*4o = 2*iv

            t  = new Timer() { Enabled = false, Interval = 10 };
            t2 = new Timer() { Enabled = true, Interval = 300 };

            t2.Tick += delegate (object h, EventArgs e)
            {
                bar.Value = (int)(10000f * cnt / fileList.Length);
                text.Text = ((bar.Value / 100) + "%").PadLeft(4, ' '); 
                Refresh();
                Invalidate(true);
                Update();
                Refresh();
            };

            t.Tick += delegate (object h, EventArgs e)
            {
                t.Enabled = false;
                if (fileList[cnt] != "version.txt")
                {
                    using (var w = new WebClient())
                    {
                        foreach (var i in Path.GetInvalidPathChars())
                            fileList[cnt] = fileList[cnt].Replace(i + "", "");
                        string cc = localDL + fileList[cnt];
                        if (!Directory.Exists(Directory.GetParent(cc).ToString()))
                            Directory.CreateDirectory(Directory.GetParent(cc).ToString());
                        if (!File.Exists(cc)) File.CreateText(cc).Dispose();
                        w.DownloadFile(new Uri(root + fileList[cnt]), cc);
                    }

                    cnt++;
                    if (cnt == fileList.Length)
                    {
                        using (var w = new WebClient())
                        { 
                            if (!File.Exists(localDL + "version.txt"))
                                File.CreateText(localDL + "version.txt").Dispose();
                            w.DownloadFile(new Uri(root + "version.txt"), localDL + "version.txt");
                        }
                        Process.Start(Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()) + @"\start.exe");
                        
                        Environment.Exit(1);
                    }
                }  
                t.Enabled = true; 
            };

            t.Enabled = true;
        }  
    }
}
