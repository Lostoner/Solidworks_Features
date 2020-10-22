using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test;

namespace solidworks_plugin
{
    public class doc_class
    {
        public static ISldWorks SwApp { get; private set; }

        public static ISldWorks ConnectToSolidWorks()
        {
            if(SwApp != null)
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
            if(SwApp != null)
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

        public static void OpenDocument()
        {
            // string DocPath = @"D:\F\三维模型库\CAD模型库\[130616]Previous_model_set\26_Wheel\DEFAULT_12190-NY-Wheel(NY-150-50-60-20).sldprt";
            string DocPath = @"D:\G\新建文件夹\零件2.sldprt";
            //string partDefaultTemplate = SwApp.GetDocumentTemplate((int)swDocumentTypes_e.swDocPART, "", 0, 0, 0);

            SwApp.OpenDoc(DocPath, (int)swDocumentTypes_e.swDocPART);
            SwApp.SendMsgToUser("Open file complete.");

        }

        public static void TraverseFeature(Feature Fea, bool TopLevel)
        {
            Feature curFea = default(Feature);
            curFea = Fea;

            while(curFea != null)
            {
                Debug.Print(curFea.Name);

                Feature subfeat = default(Feature);
                subfeat = (Feature)curFea.GetFirstSubFeature();

                while(subfeat != null)
                {
                    TraverseFeature(subfeat, false);
                    Feature nextSubFeat = default(Feature);
                    nextSubFeat = (Feature)subfeat.GetNextSubFeature();
                    subfeat = nextSubFeat;
                    nextSubFeat = null;
                }

                subfeat = null;

                Feature nextFeat = default(Feature);

                if(TopLevel)
                {
                    nextFeat = (Feature)curFea.GetNextFeature();
                }
                else
                {
                    nextFeat = null;
                }

                curFea = nextFeat;
                nextFeat = null;
            }
        }

        public static string[] GetAllFile(string path)
        {
            string[] files = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);

            return files;
        }

        public static void AutoFeature()
        {
            ISldWorks swApp = ConnectToSolidWorks();
            var swModel = (ModelDoc2)swApp.ActiveDoc;


        }

        public static void DifferenceToFeatures()
        {
            ISldWorks swApp = ConnectToSolidWorks();
            var swModel = (ModelDoc2)swApp.ActiveDoc;

            Feature swFeat = (Feature)swModel.FirstFeature();
            //Feature subFeat = (Feature)swFeat.GetFirstSubFeature();

            Feature nextFeat = default(Feature);
            while(swFeat != null)
            {
                Debug.Print(swFeat.Name);
                nextFeat = swFeat.GetNextFeature();
                swFeat = nextFeat;
                nextFeat = null;
            }
        }

        public static void TrueFeature_NameOnly()
        {
            ISldWorks swApp = ConnectToSolidWorks();
            var swModel = (ModelDoc2)swApp.ActiveDoc;

            Feature swFeat = (Feature)swModel.FirstFeature();
            //int i = 0;
            while(swFeat != null)
            {
                Feature SubFeature = swFeat.GetFirstSubFeature();
                while(SubFeature != null)
                {
                    if(SubFeature.GetTypeName2() == "ProfileFeature")
                    {
                        Sketch swSketch = SubFeature.GetSpecificFeature2();
                        //i++;
                        //Debug.Print("\: ", i);

                        //string skePrint = swSketch.ToString();
                        string skePrint = SubFeature.Name;
                        Debug.Print(skePrint);
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

        public static void OpenAndClose()
        {
           
            ModelDoc2 swModel = default(ModelDoc2);
            FeatureManager featureManager = default(FeatureManager);

            ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;

            object[] featureList = null;
            featureManager = (FeatureManager)swModel.FeatureManager;
            featureList = featureManager.GetFeatures(false);
            foreach(Feature f in featureList)
            {
               
                if (f.Name.Equals("拉伸3"))
                {
                    Debug.Print(f.GetOwnerFeature().Name);
                }
            }
        }

        //********************************************


        static Dictionary<string, int> dictionary = new Dictionary<string, int>();
        static Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
        static Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
        static MTreeNode head = new MTreeNode();

        public static void findRelations3()
        {
            ModelDoc2 swModel = default(ModelDoc2);
            FeatureManager featureManager = default(FeatureManager);
            ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            object[] featureList = null;
            featureManager = (FeatureManager)swModel.FeatureManager;
            featureList = featureManager.GetFeatures(true);
            int k = 0;
            for(int i = 17; i < featureList.Count(); i++)
            {
                Feature feature = (Feature)featureList[i];
                if (feature.GetTypeName().Equals("ProfileFeature")) continue;
                dictionary1[feature.Name] = k;
                dictionary2[k++] = feature.Name;
            }
            for(int i = 0; i < k; i++)
            {
                Debug.Print(i+"---->"+dictionary2[i]);
            }
            Debug.Print("-------------------");
            Graph graph = new Graph(k);
            for (int i = 17; i < featureList.Count(); i++)
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
                object[] fatherFeatureList = feature.GetParents();
                if (fatherFeatureList == null) continue;
                for(int j = 0; j < fatherFeatureList.Length; j++)
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

        public static void findRelations2()
        {
            ModelDoc2 swModel = default(ModelDoc2);
            FeatureManager featureManager = default(FeatureManager);
            ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            object[] featureList = null;
            featureManager = (FeatureManager)swModel.FeatureManager;
            featureList = featureManager.GetFeatures(true);
            int k = featureList.Length - 17;
            Debug.Print("K" + k);
            List<List<int>> ans = new List<List<int>>(k);
            //将特征存入字典
            for(int i = 17; i < featureList.Length; i++)
            {
                Feature f = (Feature)featureList[i];
                dictionary1[f.Name] = i;
                dictionary2[i] = f.Name;
            }
            //无子关系
            dictionary1["None"] = 0;
            dictionary2[0] = "None";
            for(int i = 17; i < featureList.Length; i++)
            {
                Feature Feature = (Feature)featureList[i];
                object[] subFeatureList = Feature.GetChildren();
                List<int> son = new List<int>();
                if (subFeatureList == null)
                {
                    //没有子关系
                    son.Add(0);
                    ans.Add(son);
                    continue;
                }
                for (int j = 0; j < subFeatureList.Length; j++)
                {
                    Feature subFeature = (Feature)subFeatureList[j];
                    if (dictionary1.ContainsKey(subFeature.Name))
                    {
                        int value = dictionary1[subFeature.Name];
                        son.Add(value);
                    }
                }
                ans.Add(son);
            }
            showImg(ans);
        }

        public static void showImg(List<List<int>> list)
        {
            int count = list.Count;
            Debug.Print(count+"");
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for(int i = 0; i < list.Count; i++)
            {
                string parentName = dictionary2[i + 17];
                Debug.Write(parentName + ":");
                for(int j = 0; j < list[i].Count; j++)
                {
                    /*string name = dictionary2[list[i][j]];
                    if (!dic.ContainsKey(name))
                    {
                        //标记
                        dic[dictionary2[list[i][j]]] = 1;
                        Debug.Write(name+" ");
                    }*/
                    string name = dictionary2[list[i][j]];
                    Debug.Write(name + "|");
                }
                
                Debug.Print("");
            }
        }

        public static void dfs(Feature feature, Feature fatherFeature)
        {
            if (dictionary.ContainsKey(feature.Name))return;
            int flag = 0;
            string fathername = fatherFeature.Name;
            object[] subFeatureArray = feature.GetChildren();
            object[] fatherSubFeatureArray = fatherFeature.GetChildren();
            List<Feature> subFeatureList = new List<Feature>();
            List<Feature> fatherSubFeatureList = new List<Feature>();
            //数组转成集合
            if (subFeatureArray != null)
            {
                for(int i = 0; i < subFeatureArray.Length; i++)
                {
                    subFeatureList.Add((Feature)subFeatureArray[i]);
                }
            }
            if (fatherSubFeatureArray != null)
            {
                for (int i = 0; i < fatherSubFeatureArray.Length; i++)
                {
                    fatherSubFeatureList.Add((Feature)fatherSubFeatureArray[i]);
                }
            }
            
            //判断兄弟中有无自己的孩子
            for (int i=0;i< fatherSubFeatureList.Count;i++)
            {
                //找到兄弟中的孩子
                MTreeNode temp = null;
                temp = MTreeNode.search_node_r(fatherSubFeatureList[i].Name, head);
                //如果有
                if (subFeatureList.Contains(fatherSubFeatureList[i])&&temp!=null)
                {
                    //Debug.Print(temp.Name);
                    //创建当前特征的结点
                    MTreeNode cur = new MTreeNode();
                    //将当前结点兄弟中是自己孩子的结点加入到自己孩子结点中
                    cur.Name = feature.Name;
                    cur.Children.Add(temp);
                    //将自己放入父节点的孩子结点中
                    MTreeNode fatherNode = MTreeNode.search_node_r(fatherFeature.Name, head);
                    if (fatherNode != null)
                    {
                        //TODO:count
                        //删除父节点中当前结点的孩子结点
                        Debug.Print("删除之前孩子节点数目" + fatherNode.Children.Count);
                        fatherNode.Children.Remove(temp);
                        Debug.Print("删除之后孩子节点数目" + fatherNode.Children.Count);
                        fatherNode.Children.Add(cur);
                        Debug.Print("添加之后孩子节点数目" + fatherNode.Children.Count);
                        //标记
                        dictionary[feature.Name] = 1;
                        flag = 1;
                    }     
                }
            }
            if (flag == 0)
            {
                MTreeNode.addNode(ref head, fathername, feature.Name,ref dictionary);
            }
           
            if (subFeatureList == null) return;
            for(int i = subFeatureList.Count - 1; i >= 0; i--)
            {
                Feature subFeature = (Feature)subFeatureList[i];
                dfs(subFeature, feature);
            }
        }
       
        public static void findRelations()
        {
            ModelDoc2 swModel = default(ModelDoc2);
            FeatureManager featureManager = default(FeatureManager);

            ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;
   
            object[] featureList = null;
            featureManager = (FeatureManager)swModel.FeatureManager;
            featureList = featureManager.GetFeatures(false);

            //
            head.Name = "拉伸1";
            //
            foreach (Feature f in featureList)
            {
                Debug.Print(f.Name + ":" + f.GetTypeName());
                //Debug.Print("特征:"+f.Name);
                /*object[] subFeatureList = null;
                object[] parentFeatureList = null;
                subFeatureList = f.GetChildren();
                parentFeatureList = f.GetParents();

                if (subFeatureList != null)
                {
                    for(int i = subFeatureList.Length-1; i >=0 ; i--)
                    {
                        Feature subFeature = (Feature)subFeatureList[i];
                        dfs(subFeature,f);
                    }
                    //Debug.Print(Microsoft.VisualBasic.Information.TypeName(children[k]));  
                }
                else
                {
                    Debug.Print(f.Name);
                }*/
            }
            //MTreeNode.f1(head,1);
        }


        public static void DoFunc()
        {
            ModelDoc2 swModel = default(ModelDoc2);
            SelectionMgr swSelMgr = default(SelectionMgr);
            SelectData swSelData = default(SelectData);
            Feature swFeat = default(Feature);
            Sketch swSketch = default(Sketch);
            SketchRelationManager swSkRelMgr = default(SketchRelationManager);
            SketchRelation swSkRel = default(SketchRelation);
            DisplayDimension dispDim = default(DisplayDimension);
            
            

            object[] vSkRelArr = null;
            int[] vEntTypeArr = null;
            object[] vEntArr = null;
            object[] vDefEntArr = null;
            SketchSegment swSkSeg = default(SketchSegment);
            SketchPoint swSkPt = default(SketchPoint);
            int i = 0;
            int j = 0;
            bool bRet = false;

            ISldWorks swApp = ConnectToSolidWorks();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swSelData = (SelectData)swSelMgr.CreateSelectData();
            swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
            swSketch = (Sketch)swFeat.GetSpecificFeature2();
            swSkRelMgr = swSketch.RelationManager;



            swModel.ClearSelection2(true);

            Debug.Print("File = " + swModel.GetPathName());
            Debug.Print("  Feat = " + swFeat.Name);

            vSkRelArr = (object[])swSkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
            if ((vSkRelArr == null))
                return;
            foreach (SketchRelation vRel in vSkRelArr)
            {
                swSkRel = (SketchRelation)vRel;

                Debug.Print("    Relation(" + i + ")");
                Debug.Print("      Type         = " + swSkRel.GetRelationType());

                dispDim = (DisplayDimension)swSkRel.GetDisplayDimension();
                if (dispDim != null)
                {
                    Debug.Print("      Display dimension         = " + dispDim.GetNameForSelection());
                }

                vEntTypeArr = (int[])swSkRel.GetEntitiesType();
                vEntArr = (object[])swSkRel.GetEntities();

                vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if ((vDefEntArr == null))
                {
                }
                else
                {
                    Debug.Print("    Number of definition entities in this relation: " + vDefEntArr.GetUpperBound(0));
                }

                if ((vEntTypeArr != null) & (vEntArr != null))
                {

                    if (vEntTypeArr.GetUpperBound(0) == vEntArr.GetUpperBound(0))
                    {
                        j = 0;

                        foreach (swSketchRelationEntityTypes_e vType in vEntTypeArr)
                        {
                            Debug.Print("        EntType    = " + vType);

                            switch (vType)
                            {
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Unknown:
                                    Debug.Print("Not known");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_SubSketch:
                                    Debug.Print("SubSketch");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point:
                                    swSkPt = (SketchPoint)vEntArr[j];
                                    Debug.Assert((swSkPt != null));
                                    Debug.Print("          SkPoint ID = [" + ((int[])(swSkPt.GetID()))[0] + ", " + ((int[])(swSkPt.GetID()))[1] + "]");
                                    bRet = swSkPt.Select4(true, swSelData);
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Line:
                                    Debug.Print("Line");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Arc:
                                    Debug.Print("Arc");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Ellipse:
                                    Debug.Print("Ellipse");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Parabola:
                                    Debug.Print("Parabola");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Spline:
                                    swSkSeg = (SketchSegment)vEntArr[j];
                                    Debug.Print("          SkSeg   ID = [" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]");
                                    bRet = swSkSeg.Select4(true, swSelData);
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Hatch:
                                    Debug.Print("Hatch");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Text:
                                    Debug.Print("Text");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Plane:
                                    Debug.Print("Plane");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Cylinder:
                                    Debug.Print("Cylinder");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Sphere:
                                    Debug.Print("Sphere");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Surface:
                                    Debug.Print("Surface");
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Dimension:
                                    Debug.Print("Dimension");
                                    break;
                                default:
                                    Debug.Print("Something else");
                                    break;
                            }

                            j = j + 1;

                        }
                    }
                }

                i = i + 1;

            }

        }



        public SldWorks swApp;

        //***********************************************
    }
}
