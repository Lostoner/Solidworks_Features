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

        public static void TestFunction(string FilePath)
        {
            Console.WriteLine(FilePath);
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
        static Dictionary<string, int> dictionary1 = new Dictionary<string, int>();         //存储文件中的所有Feature，以“名-序号”的键值对字典存储
        static Dictionary<int, string> dictionary2 = new Dictionary<int, string>();         //存储文件中的所有Feature，以“序号-名”的键值对字典存储
        //static MTreeNode head = new MTreeNode();

        public static void findRelations3(ISldWorks swApp, TextBox textBox1)
        {
            ModelDoc2 swModel = default(ModelDoc2);
            FeatureManager featureManager = default(FeatureManager);
            //ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;                                           //获取当前激活的模型
            object[] featureList = null;
            featureManager = (FeatureManager)swModel.FeatureManager;
            featureList = featureManager.GetFeatures(true);                                  //获取当前模型的Feature
            int k = 0;
            for (int i = 17; i < featureList.Count(); i++)
            {
                Feature feature = (Feature)featureList[i];
                if (feature.GetTypeName().Equals("ProfileFeature")) continue;           //判定是否为所需类型的Feature
                dictionary1[feature.Name] = k;                                                         //通过判定则存入两个字典中
                dictionary2[k++] = feature.Name;
            }
            for (int i = 0; i < k; i++)                                                                         //输出相关信息
            {
                Debug.Print(i + "---->" + dictionary2[i]);
                textBox1.AppendText(i + "---->" + dictionary2[i]+"\n");
            }
            Debug.Print("-------------------");
            textBox1.AppendText("-------------------\n");                                         //输出相关信息
            Graph graph = new Graph(k);
            for (int i = 17; i < featureList.Count(); i++)                                              //遍历各Feature，找出其子Feature，存入图结构
            {
                Feature feature = (Feature)featureList[i];
                if (feature.GetTypeName().Equals("ProfileFeature")) continue;
                int father = dictionary1[feature.Name];
                //结点存入特征
                graph.AdjList[father].feature = feature;
                //连接子关系
                object[] subFeatureList = feature.GetChildren();
                if (subFeatureList != null)
                {
                    for (int j = 0; j < subFeatureList.Length; j++)
                    {
                        Feature subFeature = (Feature)subFeatureList[j];
                        if (dictionary1.ContainsKey(subFeature.Name))
                        {
                            int son = dictionary1[subFeature.Name];
                            //加入图
                            graph.add(father, son);
                        }
                    }
                }
                //结点装入草图
                object[] fatherFeatureList = feature.GetParents();                                       //草图节点相关，具体参见Mysketch.cs中的doFun()
                if (fatherFeatureList == null) continue;
                for (int j = 0; j < fatherFeatureList.Length; j++)
                {
                    Feature fatherFeature = (Feature)fatherFeatureList[j];
                    if (fatherFeature.GetTypeName().Equals("ProfileFeature"))
                    {
                        //graph.AdjList[father].sketch = fatherFeature;
                        MySketch mySketch = new MySketch(fatherFeature);
                        mySketch.doFunc();
                        graph.AdjList[father].sketch = mySketch;
                    }
                }
            }
            graph.DFSTraverse(graph);
        }

        public static void testSegments(ISldWorks swApp, TextBox textbox1)
        {
            //ISldWorks swApp = ConnectToSolidWorks();
            var swModel = (ModelDoc2)swApp.ActiveDoc;

            Feature swFeat = (Feature)swModel.FirstFeature();
            //int i = 0;
            while (swFeat != null)
            {
                Feature SubFeature = swFeat.GetFirstSubFeature();
                while (SubFeature != null)
                {
                    if (SubFeature.GetTypeName2() == "ProfileFeature")
                    {
                        Sketch swSketch = SubFeature.GetSpecificFeature2();
                        //i++;
                        //Debug.Print("\: ", i);
                        //string skePrint = swSketch.ToString();
                        //string skePrint = SubFeature.Name;
                        //Debug.Print(skePrint);
                        Loop temLoop = new Loop();
                        temLoop.storeLoop(swSketch);
                        temLoop.showLoop(textbox1);
                    }

                    Feature NextSubFeat = SubFeature.GetNextSubFeature();
                    SubFeature = NextSubFeat;
                    NextSubFeat = null;
                }

                Feature NextFea = swFeat.GetNextFeature();
                swFeat = NextFea;
                NextFea = null;
            }
        }
    }
}
