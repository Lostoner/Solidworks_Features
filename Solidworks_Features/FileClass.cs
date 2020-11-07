using SolidWorks.Interop.sldworks;
using solidworks_plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test;

namespace Solidworks_Features
{
    class FileClass
    {
        public static ISldWorks SwApp { get; private set; }

        public static void TestFunction(ISldWorks swApp)
        {
            var swModel = (ModelDoc2)swApp.ActiveDoc;

            Feature swFeat = (Feature)swModel.FirstFeature();
            while(swFeat != null)
            {
                Feature SubFeature = swFeat.GetFirstSubFeature();
                while(SubFeature != null)
                {
                    if(SubFeature.GetTypeName2() == "ProfileFeature")
                    {
                        Sketch swSketch = SubFeature.GetSpecificFeature2();
                        newSketch nSwSketch = new newSketch(swSketch);
                        nSwSketch.storePoints();
                        nSwSketch.storeSegments();
                        nSwSketch.printData();
                    }
                    
                    Feature NextSubFeat = SubFeature.GetNextSubFeature();
                    SubFeature = NextSubFeat;
                    NextSubFeat = null;
                }

                Feature NextFeat = swFeat.GetNextFeature();
                swFeat = NextFeat;
                NextFeat = null;
            }
        }

        public static ISldWorks ConnectToSolidWorks()                   //连接函数，将该程序与已经打开的solidworks窗口进行连接，仅有进行连接后才能进行之后的solidworks接口调用
        {
            if (SwApp != null)
            {
                return SwApp;
            }
            Debug.Print("Connect to solidworks...");
            try
            {
                SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch (COMException)
            {
                try
                {
                    SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application.23");//2015
                }
                catch (COMException)
                {
                    try
                    {
                        SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application.26");//2018
                    }
                    catch (COMException)
                    {
                        MessageBox.Show("Could not connect to SolidWorks.", "SolidWorks", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        SwApp = null;
                    }
                }
            }
            if (SwApp != null)
            {
                Debug.Print("Connection succeed.");
                return SwApp;
            }
            else
            {
                Debug.Print("Connection failed.");
                return null;
            }
        }

        /**********************************************/
        //static Dictionary<string, int> dictionary = new Dictionary<string, int>();
        
    }
}
