using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class Document
    {
        int numFea, numLoop, numPoint;

        List<SketchPoint> Points = new List<SketchPoint>();
        List<Feature> Feas = new List<Feature>();
        List<Loop> Loops = new List<Loop>();


        public static void printToDocument()
        {
            //把属性全部输出吧

        }
    }
}
