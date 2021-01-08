using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

[assembly: AssemblyTitle("installer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("picture")]
[assembly: AssemblyProduct("installer")]
[assembly: AssemblyCopyright("Copyright © picture 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("a16d9c7e-7c3d-40fa-a61f-213659b5f018")] 
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace installer
{
    public partial class Form1 : Form
    { 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        bool IsExist;
       

        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = AgreeText;
            label2.Text = "";

            button1.Click += delegate (object j, EventArgs e) 
            {
                var fbd = new FolderBrowserDialog()
                {
                    Description = "フォルダを指定してください。",
                    SelectedPath = @"C:\Windows"
                };

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath;
                    TextChanged();
                }
            };

            button2.Click += delegate (object j, EventArgs e) 
            {
                TextChanged();
                if (IsExist && checkBox1.Checked) MakeApp();
            };

            button3.Click += delegate (object j, EventArgs e) 
            { 
                var r = MessageBox.Show("終了しますか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (r == DialogResult.Yes)
                    Environment.Exit(1);
            };
            textBox1.TextChanged += delegate (object j, EventArgs e)
            {
                TextChanged();
            };
        }


        string RootDirName = "Games_";
        string rootdir;

        void MakeApp()
        {
            rootdir = textBox1.Text + @"\" + RootDirName + @"\";
            
            //既に同名フォルダがあるとまざるので～(1)\みたいにする------------------------
            if (Directory.Exists(rootdir))
            {
                int f = 0;
                do
                {
                    f++;
                    rootdir = textBox1.Text + @"\" + RootDirName + "(" + f + ")" + @"\";
                }
                while (Directory.Exists(rootdir));
            }

            //----------------------------------------
            MkDir(""); //ディレクトリ作成
            MkDir("System"); //ディレクトリ作成 

            //----------------------------------------
            GetFile("start.exe", "start.exe");
            GetFile("update.exe", @"System\update.exe");

            //----------------------------------------
            string[] fileList;
            using (var w = new WebClient())
                fileList = w.DownloadString("http://xs238699.xsrv.jp/picture/onlineGame_2020/GameSystem/GameRootList.txt").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int v = 0; v < fileList.Length; v++)
            {
                fileList[v] = Regex.Replace(fileList[v], @"[^\w\.@-]", "", RegexOptions.None ); 
                GetFile(fileList[v], fileList[v]);
            }

            //----------------------------------------
            Process.Start(rootdir + @"System\update.exe"); 
            Environment.Exit(0);
        }




        static string r = "http://xs238699.xsrv.jp/picture/onlineGame_2020/GameSystem/GameRoot/";

        void MkDir(string c) { if(!Directory.Exists(rootdir+c))  Directory.CreateDirectory(rootdir+c);  }

        void GetFile(string x, string f)
        { 
            try
            {
                x = x.Replace("\n", "");
                using (var w = new WebClient())
                {
                    if (!File.Exists(rootdir + f))
                        File.CreateText(rootdir + f).Dispose();
                    w.DownloadFile(new Uri((r + x).Replace(@"\", "/")), rootdir + f);
                }
            }
            catch (Exception) {}
        }

        void TextChanged()
        {
            IsExist = Directory.Exists(textBox1.Text);
            ERRORTEXT();
        }

        

        void ERRORTEXT()
        {
            label2.Text =
                (!IsExist ? "存在しないディレクトリです.\n" : "") + 
                (!checkBox1.Checked ? "同意ボタンが押されていません." : "");
        }


        string AgreeText =
//--------------------------------|ここまで
$@"「I agree with the terms」左部のチェックボックスにチェックを入れることは
インストールおよびプレイに際し以下の項目をすべて了承したとみなします。

1.本ゲームはPicture[久留米工業大学学生チーム](以降""開発元"")の
  作成したコンテンツであり二次配布などを禁止します。 
2.本ソフトウェアの改変、解析などを禁止します。
3.本コンテンツにおいて開発元は一切の責任を負いません。
4.ゲームインストール後、アカウントを作成していただきますが、
  他人のアカウントを無断で使用したり、悪用することを禁止します。
5.オンライン上にて他ユーザに不快感を与える発言(人種差別や特定の個人
  を貶めるような発言、性的な発言など)を禁止します。
";

    }

}
