using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace solidworks_plugin
{

    public partial class Form1 : Form
    {
        string path = @"D:\F\三维模型库\Parts_WithFeature";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ISldWorks SwApp = doc_class.ConnectToSolidWorks();
            if(SwApp != null)
            {
                string msg = "This message from C#. solidworks version is " + SwApp.RevisionNumber();
                SwApp.SendMsgToUser(msg);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doc_class.OpenDocument();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = doc_class.ConnectToSolidWorks();

            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                Feature swFeat = (Feature)swModel.FirstFeature();

                doc_class.TraverseFeature(swFeat, true);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] files;

            files = doc_class.GetAllFile(path);

            foreach(string f in files)
            {
                Debug.Print(f);
                Debug.Print("\n");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            doc_class.DifferenceToFeatures();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            doc_class.TrueFeature_NameOnly();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
                doc_class.OpenAndClose();
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            doc_class.DoFunc();
        }

        private void button9_Click(object sender, EventArgs e) => doc_class.findRelations3();
    }
}
