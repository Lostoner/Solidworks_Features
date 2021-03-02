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
        public List<newSketch> skets;                   //草图数组
        public ISldWorks swApp;                           //该模型文件的源数据结构
        public string fileName;                              //该模型文件的文件名
        public List<newFeature> feas;                   //Feature数组
        public List<string> Types;                          //常量数组，保存所有Feature的类型名以筛选有效的Feature

        public FileClass(string fil)                            //FileClass类初始化函数
        {
            skets = new List<newSketch>();
            fileName = fil;
            swApp = ConnectToSolidWorks();
            swApp.OpenDoc(fil, (int)swDocumentTypes_e.swDocPART);
            feas = new List<newFeature>();
            Types = new List<string>();
            Types.Add("BaseBody");
            Types.Add("Blend");
            Types.Add("BlendCut");
            Types.Add("Boss");
            Types.Add("BossThin");
            Types.Add("Cut");
            Types.Add("CutThin");
            Types.Add("Extrusion");
            Types.Add("NetBlend");
            Types.Add("RevCut");
            Types.Add("Revolution");
            Types.Add("RevolutionThin");
            Types.Add("Sweep");
            Types.Add("SweepCut");
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
        }                           //打印，测试函数

        public void TestFunction()                  //文件数据读取函数
        {
            var swModel = (ModelDoc2)swApp.ActiveDoc;           //获取当前打开的文件

            Feature swFeat = (Feature)swModel.FirstFeature();     //获取首个Feature
            while(swFeat != null)                                                   //遍历所有Feature
            {
                Feature NextFeat;
                if (!Types.Contains(swFeat.GetTypeName2()))         //判断Feature是否为有效的Feature
                {
                    //Debug.Print("name: " + swFeat.Name + "/ type: " + swFeat.GetTypeName2());
                    NextFeat = swFeat.GetNextFeature();
                    swFeat = NextFeat;
                    NextFeat = null;
                    continue;
                }

                //Debug.Print("name: " + swFeat.GetTypeName2());

                newFeature fea = new newFeature(swFeat);            //将Feature保存在自定义的Feature结构中，初步提取数据

                Debug.Print(fea.ori.GetTypeName());
                fea.getFeaData();

                Feature SubFeature = swFeat.GetFirstSubFeature();
                while(SubFeature != null)                                        //遍历草图
                {
                    if(SubFeature.GetTypeName2() == "ProfileFeature")
                    {
                        Sketch swSketch = SubFeature.GetSpecificFeature2();
                        Debug.Print(SubFeature.Name);
                        newSketch nSwSketch = new newSketch(swSketch);
                        //nSwSketch.storePoints();
                        nSwSketch.storeSegments2();                         //获取提取环之前所需的草图中的点数组与边数组
                        //nSwSketch.printData();
                        nSwSketch.getLoop();                                     //获取环函数，详情查看newSketch.cs文件
                        nSwSketch.changePois(swApp);                      //将所有点转化为绝对坐标系下的坐标

                        nSwSketch.printLoop();                                  //打印环，测试用

                        skets.Add(nSwSketch);                                   //将草图加入草图数组
                        fea.sketchs.Add(skets.Count - 1);                   //将草图索引加入对应的Feature中
                    }
                    
                    Feature NextSubFeat = SubFeature.GetNextSubFeature();           //遍历草图常规操作
                    SubFeature = NextSubFeat;
                    NextSubFeat = null;
                }

                //fea.getExtrude();

                feas.Add(fea);                                                        //将提取完毕的Feature加入Feature数组

                NextFeat = swFeat.GetNextFeature();                     //Feature常规遍历操作
                swFeat = NextFeat;
                NextFeat = null;
            }

            for(int i = 0; i < feas.Count; i++)                               //遍历获取所有Feature的子Feature保存在各自的数据结构中
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
        }                                                           //关闭模型文件

        
    }
}
