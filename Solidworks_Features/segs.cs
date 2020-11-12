using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class segs
    {
        public class Line
        {
            SketchLine seg;
            public SketchSegment ori;
            public int sPoint, ePoint;

            public Line(SketchSegment segments)
            {
                seg = (SketchLine)segments;
                ori = segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }

            public bool same(segs.Line exist)
            {
                if(exist.sPoint == sPoint && exist.ePoint == ePoint)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /*
            public segs.Line frag(newPoint startP, newPoint endP)
            {
                Line tem = new Line();
                return tem;
            }
            */

            public int getAnother(int index)
            {
                if(index == ePoint)
                {
                    return sPoint;
                }
                else if(index == sPoint)
                {
                    return ePoint;
                }
                else
                {
                    return -1;
                }
            }
        }

        public class Ellipse
        {
            SketchEllipse seg;
            SketchSegment ori;
            public int sPoint, ePoint, cPoint;

            public Ellipse(SketchSegment segments)
            {
                seg = (SketchEllipse)segments;
                ori = segments;
            }

            public void setPoint(int start, int end,  int center)
            {
                sPoint = start;
                ePoint = end;
                cPoint = center;
            }

            public bool same(segs.Ellipse exist)
            {
                if (exist.sPoint == sPoint && exist.ePoint == ePoint &&exist.cPoint == cPoint)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class Arc
        {
            SketchArc seg;
            SketchSegment ori;
            public int sPoint, ePoint;

            public Arc(SketchSegment segments)
            {
                seg = (SketchArc)segments;
                ori = segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }

            public bool same(segs.Arc exist)
            {
                if (exist.sPoint == sPoint && exist.ePoint == ePoint)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class Parabola
        {
            SketchParabola seg;
            SketchSegment ori;
            public int sPoint, ePoint;

            public Parabola(SketchSegment segments)
            {
                seg = (SketchParabola)segments;
                ori = segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }

            public bool same(segs.Parabola exist)
            {
                if (exist.sPoint == sPoint && exist.ePoint == ePoint)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class Spline
        {
            SketchSpline seg;
            SketchSegment ori;
            public List<int> sPoints;
            public int len;
            public int sPoint, ePoint;
            
            public Spline(SketchSegment segments)
            {
                seg = (SketchSpline)segments;
                ori = segments;
                sPoints = new List<int>();
                len = 0;
            }

            public void setPoint(int index)
            {
                sPoints.Add(index);
                len++;
                if(len == 1)
                {
                    sPoint = index;
                }
                else
                {
                    ePoint = index;
                }
            }

            public bool same(segs.Spline exist)
            {
                int count = 0;
                for(int i = 0; i < sPoints.Count; i++)
                {
                    if(sPoints[i] == exist.sPoints[i])
                    {
                        count++;
                    }
                }
                if(count == sPoints.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
