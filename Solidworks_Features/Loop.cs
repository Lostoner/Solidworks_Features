﻿using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;
using System.Diagnostics;

namespace Solidworks_Features
{

    class Loop
    {
        int num_poi;
        int num_seg;
        List<SketchSegment> segs = new List<SketchSegment>();
        List<SketchPoint> pois = new List<SketchPoint>();

        public Loop()
        {

        }

        static public void store(Sketch ske)
        {
            object[] segments = ske.GetSketchSegments();

        }
    }
}
