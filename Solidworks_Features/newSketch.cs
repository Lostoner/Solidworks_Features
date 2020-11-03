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
        List<SketchSegment> segs = new List<SketchSegment>();
        List<SketchPoint> pois = new List<SketchPoint>();

        public newSketch(Sketch ske)
        {
            object[] segments = ske.GetSketchSegments();
            for(int i = 0; i < segments.Length; i++)
            {

            }
        }
    }
}
