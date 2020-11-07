using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<segs.Arc> segArc;
        public List<segs.Line> segLin;
        public List<segs.Ellipse> segEll;
        public List<segs.Parabola> segPar;
        public List<segs.Spline> segSpl;

        public void storePoints()
        {
            Debug.Print("Storing points: ");
            object[] temPoi = sket.GetSketchPoints2();
            for(int i = 0; i < temPoi.Length; i++)
            {
                newPoint tem = new newPoint((SketchPoint)temPoi[i]);
                tem.setIndex(i);
                idToIndex.Add(tem.ID, tem.index);
                indexToId.Add(tem.index, tem.ID);
                pois.Add(tem);
                Debug.Print("Point " + i.ToString() + " : (" + tem.ID.Key + ", " + tem.ID.Value + ")");
                Debug.Print(tem.x + ", " + tem.y + ", " + tem.z);
            }
        }

        public void storeSegments()
        {
            Debug.Print("Storing segments: ");
            int count = 0;
            object[] segments = sket.GetSketchSegments();
            foreach(SketchSegment seg in segments)
            {
                int type = seg.GetType();
                switch(type)
                {
                    case 0:
                        count++;
                        segs.Line temLine = new segs.Line(seg);
                        SketchLine line = (SketchLine)seg;
                        temLine.setPoint(findPoint(line.GetStartPoint2()), findPoint(line.GetEndPoint2()));
                        segLin.Add(temLine);
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(line.GetStartPoint2()) + "->" + findPoint(line.GetEndPoint2()) + ")");
                        break;
                    case 1:
                        segs.Arc temArc = new segs.Arc(seg);
                        SketchArc arc = (SketchArc)seg;
                        temArc.setPoint(findPoint(arc.GetStartPoint2()), findPoint(arc.IGetEndPoint2()));
                        segArc.Add(temArc);
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(arc.GetStartPoint2()) + "->" + findPoint(arc.GetEndPoint2()) + ")");
                        break;
                    case 2:
                        segs.Ellipse temEllipse = new segs.Ellipse(seg);
                        SketchEllipse ellipse = (SketchEllipse)seg;
                        temEllipse.setPoint(findPoint(ellipse.GetStartPoint2()), findPoint(ellipse.GetEndPoint2()), findPoint(ellipse.GetCenterPoint2()));
                        segEll.Add(temEllipse);
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(ellipse.GetStartPoint2()) + "->" + findPoint(ellipse.GetEndPoint2()) + ")");
                        break;
                    case 3:
                        segs.Spline temSpline = new segs.Spline(seg);
                        SketchSpline spline = (SketchSpline)seg;
                        SketchPoint[] tempoints = spline.GetPoints2();
                        for(int i = 0; i < tempoints.Length; i++)
                        {
                            int index = findPoint(tempoints[i]);
                            temSpline.setPoint(index);
                        }
                        segSpl.Add(temSpline);
                        //Debug.Print("Spline");
                        break;
                    case 5:
                        segs.Parabola temParabola = new segs.Parabola(seg);
                        SketchParabola parabola = (SketchParabola)seg;
                        temParabola.setPoint(findPoint(parabola.GetStartPoint2()), findPoint(parabola.IGetEndPoint2()));
                        segPar.Add(temParabola);
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(parabola.GetStartPoint2()) + "->" + findPoint(parabola.GetEndPoint2()) + ")");
                        break;
                    case 4:
                        break;
                }
            }
        }

        public int findPoint(SketchPoint point)
        {
            KeyValuePair<int, int> ID = new KeyValuePair<int, int>(point.GetID()[0], point.GetID()[1]);
            //int id1 = point.GetID()[0];
            //int id2 = point.GetID()[1];
            for(int i = 0; i < pois.Count; i++)
            {
                if(ID.Key == pois[i].ID.Key && ID.Value == pois[i].ID.Value)
                {
                    return pois[i].index;
                }
            }
            return -1;
        }

        public void printData()
        {
            for(int i = 0; i < segLin.Count; i++)
            {
                Debug.Print("Line " + i + ": " + segLin[i].sPoint + "->" + segLin[i].ePoint);
            }

            for (int i = 0; i < segArc.Count; i++)
            {
                Debug.Print("Arc " + i + ": " + segArc[i].sPoint + "->" + segArc[i].ePoint);
            }

            for (int i = 0; i < segEll.Count; i++)
            {
                Debug.Print("Ellipse " + i + ": " + segEll[i].sPoint + "->" + segEll[i].ePoint);
            }

            for (int i = 0; i < segPar.Count; i++)
            {
                Debug.Print("Parabola " + i + ": " + segPar[i].sPoint + "->" + segPar[i].ePoint);
            }

            for(int i = 0; i < segSpl.Count; i++)
            {
                Debug.Print("Spline " + i + ": " + segSpl[i].sPoint + "->" + segSpl[i].ePoint);
            }
        }

        public newSketch(Sketch ske)
        {
            sket = ske;
            segs = new List<SketchSegment>();
            pois = new List<newPoint>();
            idToIndex = new Dictionary<KeyValuePair<int, int>, int>();
            indexToId = new Dictionary<int, KeyValuePair<int, int>>();
            segArc = new List<segs.Arc>();
            segLin = new List<segs.Line>();
            segEll = new List<segs.Ellipse>();
            segPar = new List<segs.Parabola>();
            segSpl = new List<segs.Spline>();
        }
    }
}
