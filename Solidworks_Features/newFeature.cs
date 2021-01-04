using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
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
        public List<int> typeData;

        public newFeature(Feature swFeat)
        {
            ori = swFeat;
            sketchs = new List<int>();
            sons = new List<int>();
            type = -1;
            typeData = new List<int>();


        }

        public void setSon(int son)
        {
            sons.Add(son);
        }

        public void setSketch(int ske)
        {
            sketchs.Add(ske);
        }
    }
}
