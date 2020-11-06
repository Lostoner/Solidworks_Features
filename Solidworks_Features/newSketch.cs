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
        public List<SketchSegment> segs;
        public List<newPoint> pois;
        public Dictionary<KeyValuePair<int, int>, int> idToIndex;
        public Dictionary<int, KeyValuePair<int, int>> indexToId;
        public List<SketchArc> segArc;
        public List<SketchLine> segLin;
        public List<SketchEllipse> segEll;
        public List<SketchParabola> segPar;
        public List<SketchSpline> segSpl;

        public void storePoints()
        {
            object[] temPoi = sket.GetSketchPoints2();
            for(int i = 0; i < temPoi.Length; i++)
            {
                newPoint tem = new newPoint((SketchPoint)temPoi[i]);
                tem.setIndex(i);
                idToIndex.Add(tem.ID, tem.index);
                indexToId.Add(tem.index, tem.ID);
                pois.Add(tem);
            }
        }

        public void storeSegments()
        {
            object[] segments = sket.GetSketchSegments();
            foreach(SketchSegment seg in segments)
            {
                int type = seg.GetType();
                switch(type)
                {
                    case 0:

                        break;

                }
            }
        }

        public int findPoint(SketchPoint point)
        {
            int id1 = point.GetID()[0];
            int id2 = point.GetID()[1];
            for(int i = 0; i < pois.Count; i++)
            {
                if()
                {
                    return pois[i].index;
                }
            }
            return -1;
        }

        public newSketch(Sketch ske)
        {
            sket = ske;
            segs = new List<SketchSegment>();
            pois = new List<newPoint>();
            idToIndex = new Dictionary<KeyValuePair<int, int>, int>();
            indexToId = new Dictionary<int, KeyValuePair<int, int>>();
            segArc = new List<SketchArc>();
            segLin = new List<SketchLine>();
            segEll = new List<SketchEllipse>();
            segPar = new List<SketchParabola>();
            segSpl = new List<SketchSpline>();
        }
    }
}
