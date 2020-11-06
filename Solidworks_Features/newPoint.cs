using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class newPoint
    {
        public KeyValuePair<int, int> ID;
        public int index;
        public double x, y, z;

        public newPoint(SketchPoint poi)
        {
            ID = new KeyValuePair<int, int>(poi.GetID()[0], poi.GetID()[1]);
            x = poi.X;
            y = poi.Y;
            z = poi.Z;
        }

        public void setIndex(int ind)
        {
            index = ind;
        }
    }
}
