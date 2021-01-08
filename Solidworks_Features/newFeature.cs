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
        public int type;
        public List<int> sketchs;
        public Feature ori;
        public List<int> sons;
        public typedata feaData;

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

        public void getExtrude()
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
