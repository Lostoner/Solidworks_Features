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
            progressBar1.Maximum = 100;
            progressBar1.Value = progressBar1.Minimum = 0;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t2 = new Thread(new ParameterizedThreadStart(function));
            t2.IsBackground = true;
            t2.Start(path);
        }

        private delegate void DeFun(int ipos);

        private void SetPos(int ipos)
        {
            if(this.progressBar1.InvokeRequired)
            {
                DeFun df = new DeFun(SetPos);
                this.Invoke(df, new object[] { ipos });
            }
            else
            {
                this.progressBar1.Value = ipos;
            }
        }

        private void function(object data)
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
                    FileClass.TestFunction(fil);
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
