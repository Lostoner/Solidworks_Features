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
        class Line
        {
            SketchSegment seg;
            int sPoint, ePoint;
            public Line(SketchSegment segments)
            {
                seg = segments;
            }
        }

        class Ellipse
        {
            SketchSegment seg;


            public Ellipse(SketchSegment segments)
            {
                seg = segments;
            }
        }

        class Arc
        {
            SketchSegment seg;
            int sPoint, ePoint;

            public Arc(SketchSegment segments)
            {
                seg = segments;
            }
        }

        class Parabola
        {
            SketchSegment seg;
            int sPoint, ePoint;

            public Parabola(SketchSegment segments)
            {
                seg = segments;
            }
        }

        class Spline
        {
            SketchSegment seg;
            
            public Spline(SketchSegment segments)
            {
                seg = segments;
            }
        }
    }
}
