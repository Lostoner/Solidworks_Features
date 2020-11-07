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
            public int sPoint, ePoint;

            public Line(SketchSegment segments)
            {
                seg = (SketchLine)segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }
        }

        public class Ellipse
        {
            SketchEllipse seg;
            public int sPoint, ePoint, cPoint;

            public Ellipse(SketchSegment segments)
            {
                seg = (SketchEllipse)segments;
            }

            public void setPoint(int start, int end,  int center)
            {
                sPoint = start;
                ePoint = end;
                cPoint = center;
            }
        }

        public class Arc
        {
            SketchArc seg;
            public int sPoint, ePoint;

            public Arc(SketchSegment segments)
            {
                seg = (SketchArc)segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }
        }

        public class Parabola
        {
            SketchParabola seg;
            public int sPoint, ePoint;

            public Parabola(SketchSegment segments)
            {
                seg = (SketchParabola)segments;
            }

            public void setPoint(int start, int end)
            {
                sPoint = start;
                ePoint = end;
            }
        }

        public class Spline
        {
            SketchSpline seg;
            public List<int> sPoints;
            public int len;
            public int sPoint, ePoint;
            
            public Spline(SketchSegment segments)
            {
                seg = (SketchSpline)segments;
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
        }
    }
}
