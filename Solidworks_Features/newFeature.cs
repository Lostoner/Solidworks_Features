using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class newFeature
    {
        public int type;                                    //该Feature的类型
        public List<int> sketchs;                      //该Feature对应的草图的索引数组
        public Feature ori;                                //该Feature的源数据
        public List<int> sons;                           //该Feature的子Feature索引数组
        public typedata feaData;                       //该Feature本身的数据，详情查看typedata.cs

        public newFeature(Feature swFeat)
        {
            ori = swFeat;
            sketchs = new List<int>();
            sons = new List<int>();
            type = -1;
            feaData = new typedata();
        }

        public void setSon(int son)
        {
            sons.Add(son);
        }

        public void setSketch(int ske)
        {
            sketchs.Add(ske);
        }

        public void getExtrude()                     //获取Feature本身的数据，目前为测试函数
        {
            IExtrudeFeatureData2 extrudeData = (IExtrudeFeatureData2)ori.GetDefinition();
            feaData.EbothDirections = extrudeData.BothDirections;
            feaData.Edepth = extrudeData.GetDepth(true);
            feaData.EreverseOffset = extrudeData.GetReverseOffset(true);
            feaData.EwallThickness = extrudeData.GetWallThickness(true);

            Debug.Print("both: " + feaData.EbothDirections);
            Debug.Print("depth: " + feaData.Edepth);
            Debug.Print("reverse: " + feaData.EreverseOffset);
            Debug.Print("wall: " + feaData.EwallThickness);
        }

        public void getRevolve()
        {

        }
    }
}
