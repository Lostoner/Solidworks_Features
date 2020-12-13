using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using solidworks_plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public List<newSketch> skets;
        public ISldWorks swApp;
        public string fileName;
        public List<newFeature> feas;

        public FileClass(string fil)
        {
            skets = new List<newSketch>();
            fileName = fil;
            swApp = ConnectToSolidWorks();
            swApp.OpenDoc(fil, (int)swDocumentTypes_e.swDocPART);
            feas = new List<newFeature>();
        }

        public static ISldWorks SwApp { get; private set; }

        public void print()
        {
            string path = @"D:\F\三维模型库\Parts_WithFeature";
            string line = "\n\n\n";
            byte[] enter = Encoding.UTF8.GetBytes(line);
            //StreamWriter sw = new StreamWriter(fileName.Substring(path.Length, fileName.Length - 7) + ".txt");
            //Debug.Print(fileName.Substring(0, fileName.Length - 7) + ".txt");
            FileStream fs = new FileStream(fileName.Substring(0, fileName.Length - 7) + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            string FeaNum = "Number of Feature: " + feas.Count + "\n";
            byte[] FeaN = Encoding.UTF8.GetBytes(FeaNum);
            fs.Write(FeaN, 0, FeaN.Length);
            for (int i = 0; i < feas.Count; i++)
            {
                string FeaSum = "Feature " + i + " " + feas[i].sketchs.Count + " " + feas[i].sons.Count + "\n";
                byte[] FeaS = Encoding.UTF8.GetBytes(FeaSum);
                fs.Write(FeaS, 0, FeaS.Length);

                string sketchInf = "";
                for(int j = 0; j < feas[i].sketchs.Count; j++)
                {
                    sketchInf += " ";
                    sketchInf += feas[i].sketchs[j];
                }
                sketchInf += "\n";
                byte[] si = Encoding.UTF8.GetBytes(sketchInf);
                fs.Write(si, 0, si.Length);

                string sonInf = "";
                for (int j = 0; j < feas[i].sons.Count; j++)
                {
                    sonInf += " ";
                    sonInf += feas[i].sons[j];
                }
                sonInf += "\n";
                byte[] sin = Encoding.UTF8.GetBytes(sonInf);
                fs.Write(sin, 0, sin.Length);
            }

            fs.Write(enter, 0, enter.Length);

            string sketNum = "Number of sketches: " + skets.Count + "\n";
            byte[] sketHead = Encoding.UTF8.GetBytes(sketNum);
            fs.Write(sketHead, 0, sketHead.Length);
            for (int i = 0; i < skets.Count; i++)
            {
                //sw.WriteLine("Sketch " + i + ", " + skets[i].loops.Count);
                string SketchNum = "Sketch " + i + " " + skets[i].loops.Count + "\n";
                byte[] SketchN = Encoding.UTF8.GetBytes(SketchNum);
                fs.Write(SketchN, 0, SketchN.Length);

                for (int j = 0; j < skets[i].loops.Count; j++)
                {
                    string LoopNum = "Loop " + j + "\n";
                    byte[] LoopN = Encoding.UTF8.GetBytes(LoopNum);
                    fs.Write(LoopN, 0, LoopN.Length);

                    //string outString = "";
                    for (int k = 0; k < skets[i].loops[j].Count; k++)
                    {
                        string outString = "";
                        outString += skets[i].loopSegs[skets[i].loops[j][k]].type;
                        outString += " ";
                        outString += skets[i].loopSegs[skets[i].loops[j][k]].start;
                        outString += " ";
                        outString += skets[i].loopSegs[skets[i].loops[j][k]].end;

                        outString += "\n";
                        byte[] OutS = Encoding.UTF8.GetBytes(outString);
                        fs.Write(OutS, 0, OutS.Length);
                    }
                }

                //sw.WriteLine("Points " + skets[i].pois.Count);
                //fs.Write(enter, 0, enter.Length);

                string PointNum = "Points " + skets[i].pois.Count + "\n";
                byte[] PointN = Encoding.UTF8.GetBytes(PointNum);
                fs.Write(PointN, 0, PointN.Length);

                //string outString2 = "";
                for(int j = 0; j < skets[i].pois.Count; j++)
                {
                    string outString2 = "";
                    outString2 += skets[i].pois[j].ox;
                    outString2 += " ";
                    outString2 += skets[i].pois[j].oy;
                    outString2 += " ";
                    outString2 += skets[i].pois[j].oz;

                    outString2 += "\n";
                    byte[] OutS2 = Encoding.UTF8.GetBytes(outString2);
                    fs.Write(OutS2, 0, OutS2.Length);
                }
                string singleLine = "\n";
                byte[] sl = Encoding.UTF8.GetBytes(singleLine);
                fs.Write(sl, 0, sl.Length);
                //sw.WriteLine(outString2);
            }
        }

        public void TestFunction()
        {
            var swModel = (ModelDoc2)swApp.ActiveDoc;

            Feature swFeat = (Feature)swModel.FirstFeature();
            while(swFeat != null)
            {
                newFeature fea = new newFeature(swFeat);

                Feature SubFeature = swFeat.GetFirstSubFeature();
                while(SubFeature != null)
                {
                    if(SubFeature.GetTypeName2() == "ProfileFeature")
                    {
                        Sketch swSketch = SubFeature.GetSpecificFeature2();
                        Debug.Print(SubFeature.Name);
                        newSketch nSwSketch = new newSketch(swSketch);
                        //nSwSketch.storePoints();
                        nSwSketch.storeSegments2();
                        //nSwSketch.printData();
                        nSwSketch.getLoop();
                        nSwSketch.changePois(swApp);

                        //nSwSketch.printLoop();

                        skets.Add(nSwSketch);
                        fea.sketchs.Add(skets.Count - 1);
                    }
                    
                    Feature NextSubFeat = SubFeature.GetNextSubFeature();
                    SubFeature = NextSubFeat;
                    NextSubFeat = null;
                }
                feas.Add(fea);

                Feature NextFeat = swFeat.GetNextFeature();
                swFeat = NextFeat;
                NextFeat = null;
            }

            for(int i = 0; i < feas.Count; i++)
            {
                object[] sonFea = feas[i].ori.GetChildren();
                if(sonFea != null)
                {
                    for (int j = 0; j < sonFea.Length; j++)
                    {
                        Feature son = (Feature)sonFea[j];
                        for (int k = 0; k < feas.Count; k++)
                        {
                            if (feas[k].ori == son)
                            {
                                feas[i].sons.Add(k);
                            }
                        }
                    }
                }
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

        public void close()
        {
            swApp.CloseDoc(fileName);
        }

        
    }
}
