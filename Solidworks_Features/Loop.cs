using SolidWorks.Interop.sldworks;
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
        List<SketchSegment> segs;
        List<SketchPoint> pois;
        public Loop()
        {
            segs = new List<SketchSegment>();
            pois = new List<SketchPoint>();
        }

        public void setStart(SketchPoint add)
        {
            if(pois.Count == 0)
            {
                pois.Add(add);
            }
            else
            {
                Debug.Print("Error wen storing first point in one loop.");
            }
        }

        public void store(SketchSegment add, SketchPoint add2)
        {
            segs.Add(add);
            pois.Add(add2);
        }

        public void delete()
        {
            segs.Remove(segs.Last());
            pois.Remove(pois.Last());
        }
    }
}
