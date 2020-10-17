using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        string path;
        int cancel = 0;

        public Form2(string f1path)
        {
            path = f1path;
            InitializeComponent();
            progressBar1.Maximum = 100;                                                               //进度条最大值
            progressBar1.Value = progressBar1.Minimum = 0;                                  //进度条最小值与当前值
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t2 = new Thread(new ParameterizedThreadStart(function));         //创建线程
            t2.IsBackground = true;
            t2.Start(path);                                                                                          //开始线程
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
            string file_path = data as string;
            string[] files = Directory.GetFiles(@"D:\F\三维模型库\Parts_WithFeature");
            int num = files.Length;
            int cou = 0;
            
            foreach(string fil in files)
            {
                Thread.Sleep(100);
                if (cancel == 0)
                {
                    cou++;
                    FileClass.TestFunction(fil);                                                                 //所调用的针对单个模型文件的操作函数，也就是后续仅需编写该函数便可。
                    SetPos((int)((float)cou / (float)num * 100));
                }
                else
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancel = 1;
        }
    }
}
