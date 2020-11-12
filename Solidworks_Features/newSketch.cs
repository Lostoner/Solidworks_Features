﻿using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solidworks_Features.segs;

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

        public int addPoint(SketchPoint point)
        {
            KeyValuePair<int, int> temID = new KeyValuePair<int, int>(point.GetID()[0], point.GetID()[1]);
            if(!idToIndex.ContainsKey(temID))
            {
                for(int i = 0; i < pois.Count; i++)
                {
                    if(point.X == pois[i].x && point.Y == pois[i].y && point.Z == pois[i].z)
                    {
                        return i;
                    }
                }

                newPoint temPoint = new newPoint(point);
                pois.Add(temPoint);
                temPoint.setIndex(pois.Count - 1);
                idToIndex.Add(temID, pois.Count - 1);
                indexToId.Add(pois.Count - 1, temID);
                return (pois.Count - 1);
            }
            else
            {
                return idToIndex[temID];
            }
        }

        public void storeSegments()
        {
            //Debug.Print("Storing segments: ");
            object[] segments = sket.GetSketchSegments();
            foreach(SketchSegment seg in segments)
            {
                bool flag = false;
                int type = seg.GetType();
                switch(type)
                {
                    case 0:
                        flag = false;
                        segs.Line temLine = new segs.Line(seg);
                        SketchLine line = (SketchLine)seg;
                        temLine.setPoint(addPoint(line.GetStartPoint2()), addPoint(line.GetEndPoint2()));
                        //temLine.setPoint(findPoint(line.GetStartPoint2()), findPoint(line.GetEndPoint2()));
                        for(int i = 0; i < segLin.Count; i++)
                        {
                            if(temLine.same(segLin[i]))
                            {
                                flag = true;
                            }
                        }
                        if(flag)
                        {
                            break;
                        }
                        else
                        {
                            segLin.Add(temLine);
                            pois[temLine.sPoint].setNext(temLine.ePoint);
                            pois[temLine.ePoint].setNext(temLine.sPoint);

                            pois[temLine.sPoint].setNextSeg(0, segLin.Count - 1);
                            pois[temLine.ePoint].setNextSeg(0, segLin.Count - 1);
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(line.GetStartPoint2()) + "->" + findPoint(line.GetEndPoint2()) + ")");
                        break;
                    case 1:
                        segs.Arc temArc = new segs.Arc(seg);
                        SketchArc arc = (SketchArc)seg;
                        temArc.setPoint(addPoint(arc.GetStartPoint2()), addPoint(arc.IGetEndPoint2()));
                        //temArc.setPoint(findPoint(arc.GetStartPoint2()), findPoint(arc.IGetEndPoint2()));
                        for (int i = 0; i < segArc.Count; i++)
                        {
                            if (temArc.same(segArc[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segArc.Add(temArc);
                            pois[temArc.sPoint].setNext(temArc.ePoint);
                            pois[temArc.ePoint].setNext(temArc.sPoint);

                            pois[temArc.sPoint].setNextSeg(1, segArc.Count - 1);
                            pois[temArc.ePoint].setNextSeg(1, segArc.Count - 1);
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(arc.GetStartPoint2()) + "->" + findPoint(arc.GetEndPoint2()) + ")");
                        break;
                    case 2:
                        flag = false;
                        segs.Ellipse temEllipse = new segs.Ellipse(seg);
                        SketchEllipse ellipse = (SketchEllipse)seg;
                        temEllipse.setPoint(addPoint(ellipse.GetStartPoint2()), addPoint(ellipse.GetEndPoint2()), addPoint(ellipse.GetCenterPoint2()));
                        //temEllipse.setPoint(findPoint(ellipse.GetStartPoint2()), findPoint(ellipse.GetEndPoint2()), findPoint(ellipse.GetCenterPoint2()));
                        for (int i = 0; i < segEll.Count; i++)
                        {
                            if (temEllipse.same(segEll[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segEll.Add(temEllipse);
                            pois[temEllipse.sPoint].setNext(temEllipse.ePoint);
                            pois[temEllipse.ePoint].setNext(temEllipse.sPoint);

                            pois[temEllipse.sPoint].setNextSeg(2, segEll.Count - 1);
                            pois[temEllipse.ePoint].setNextSeg(2, segEll.Count - 1);
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(ellipse.GetStartPoint2()) + "->" + findPoint(ellipse.GetEndPoint2()) + ")");
                        break;
                    case 3:
                        flag = false;
                        segs.Spline temSpline = new segs.Spline(seg);
                        SketchSpline spline = (SketchSpline)seg;
                        SketchPoint[] tempoints = spline.GetPoints2();
                        for (int i = 0; i < tempoints.Length; i++)
                        {
                            int index = addPoint(tempoints[i]);
                            temSpline.setPoint(index);
                        }
                        /*
                        for(int i = 0; i < tempoints.Length; i++)
                        {
                            int index = findPoint(tempoints[i]);
                            temSpline.setPoint(index);
                        }
                        */
                        for (int i = 0; i < segSpl.Count; i++)
                        {
                            if (temSpline.same(segSpl[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segSpl.Add(temSpline);
                            pois[temSpline.sPoint].setNext(temSpline.ePoint);
                            pois[temSpline.ePoint].setNext(temSpline.sPoint);

                            pois[temSpline.sPoint].setNextSeg(3, segSpl.Count - 1);
                            pois[temSpline.ePoint].setNextSeg(3, segSpl.Count - 1);
                        }
                        //Debug.Print("Spline");
                        break;
                    case 5:
                        segs.Parabola temParabola = new segs.Parabola(seg);
                        SketchParabola parabola = (SketchParabola)seg;
                        temParabola.setPoint(addPoint(parabola.GetStartPoint2()), addPoint(parabola.IGetEndPoint2()));
                        //temParabola.setPoint(findPoint(parabola.GetStartPoint2()), findPoint(parabola.IGetEndPoint2()));
                        for (int i = 0; i < segPar.Count; i++)
                        {
                            if (temParabola.same(segPar[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segPar.Add(temParabola);
                            pois[temParabola.sPoint].setNext(temParabola.ePoint);
                            pois[temParabola.ePoint].setNext(temParabola.sPoint);

                            pois[temParabola.sPoint].setNextSeg(5, segPar.Count - 1);
                            pois[temParabola.ePoint].setNextSeg(5, segPar.Count - 1);
                        }
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
            Debug.Print("------------------------Points--------------------------");
            for(int i = 0; i < pois.Count; i++)
            {
                Debug.Print("Point " + i.ToString() + " : (" + pois[i].ID.Key + ", " + pois[i].ID.Value + "); " + "index: " + pois[i].index);
                Debug.Print(pois[i].x + ", " + pois[i].y + ", " + pois[i].z);
                for(int j = 0; j < pois[i].next.Count; j++)
                {
                    Debug.Print("Point to point: ->" + pois[i].next[j]);
                    Debug.Print("Point to segment: ->" + pois[i].nextSeg[j]);
                }
            }
            Debug.Print("----------------------Segments------------------------");
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

        public void getLoop()
        {
            while(pois.Count > 0)
            {

            }
        }

        public bool dfs(int ori, int index, KeyValuePair<int, int> seg, Loop unfinished)
        {
            switch(seg.Key)
            {
                case 0:
                    Line temseg = segLin[seg.Value];
                    int otherP = temseg.getAnother(index);
                    if(otherP != ori)
                    {
                        if (pois[otherP].nextSeg.Count - 1 != 0)
                        {
                            unfinished.store(temseg.ori, pois[otherP].ori);
                            for(int i = 0; i < pois[otherP].nextSeg.Count; i++)
                            {
                                if(!pois[otherP].nextSeg[i].Equals(seg))
                                {
                                    bool flag = dfs(ori, otherP, pois[otherP].nextSeg[i], unfinished);
                                    if (flag)
                                    {
                                        if(pois[otherP].nextSeg.Count == 2)
                                        {

                                        }
                                        return true;
                                    }
                                    else
                                    {
                                        unfinished.delete();
                                        return false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    break;
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
