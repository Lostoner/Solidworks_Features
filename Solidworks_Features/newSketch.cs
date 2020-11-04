using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class newSketch
    {
        Sketch sket;
        List<SketchSegment> segs = new List<SketchSegment>();
        List<newPoint> pois = new List<newPoint>();

        public void storePoints()
        {
            object[] points = sket.GetSketchPoints2();
            for(int i = 0; i < points.Length; i++)
            {
                newPoint tem = new newPoint((SketchPoint)points[i]);
                tem.setIndex(i);
                pois.Add(tem);
            }
        }

        public void storeSegments()
        {
            object[] segments = sket.GetSketchSegments();
            for(int i = 0; i < segments.Length; i++)
            {

            }
        }

        public int findPoint(SketchPoint point)
        {
            int id1 = point.GetID()[0];
            int id2 = point.GetID()[1];
            for(int i = 0; i < pois.Count; i++)
            {
                if(pois[i].ori1 == id1 && pois[i].ori2 == id2)
                {
                    return pois[i].index;
                }
            }
            return -1;
        }

        public newSketch(Sketch ske)
        {
            sket = ske;
        }
    }
}
