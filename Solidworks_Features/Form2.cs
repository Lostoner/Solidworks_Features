using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solidworks_Features
{
    public partial class Form2 : Form
    {
        public string path;
        int cancel = 1;

        public Form2(string f1path)
        {
            this.path = f1path;
            InitializeComponent();
            progressBar1.Maximum = 1000;                                                               //进度条最大值
            progressBar1.Value = progressBar1.Minimum = 0;                                  //进度条最小值与当前值
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t2 = new Thread(new ParameterizedThreadStart(function));         //创建线程
            t2.IsBackground = true;
            t2.Start(this.path);                                                                                          //开始线程
        }

        private delegate void DeFun(int ipos);                                                         //委托，用于传参

        private void SetPos(int ipos)                                                                        //委托实例化，用于调整进度条的值
        {
            if(this.progressBar1.InvokeRequired)
            {
                DeFun df = new DeFun(SetPos);
                this.Invoke(df, new object[] { ipos });
            }
            else
            {
                this.progressBar1.Value = ipos;                                                           //更改进度条的值
            }
        }

        private void function(object data)                                                               //线程所调用的函数
        {
            string file_path = data.ToString();
            string[] files = Directory.GetFiles(file_path);
            int num = files.Length;
            int cou = 0;
            
            foreach(string fil in files)
            {
                Debug.Print("--------------------------------------------------------------------------------------------------");
                Debug.Print("--------------------------------------------------------------------------------------------------");
                Debug.Print(fil);
                if (cancel == 1)
                {
                    cou++;
                    FileClass file = new FileClass(fil);
                    //ISldWorks swApp = FileClass.ConnectToSolidWorks();
                    //swApp.OpenDoc(fil, (int)swDocumentTypes_e.swDocPART);
                    //FileClass.TestFunction(fil);                                                                 //所调用的针对单个模型文件的操作函数，也就是后续仅需编写该函数便可。
                    file.TestFunction();
                    file.print();
                    //swApp.CloseDoc(fil);
                    file.close();

                    SetPos((int)((float)cou / (float)num * 1000));
                }
                else
                {
                    //Thread.CurrentThread.Suspend();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancel = -cancel;
            if(cancel == -1)
            {
                button1.Text = "继续";
            }
            else
            {
                button1.Text = "暂停";
            }
        }
    }
}
