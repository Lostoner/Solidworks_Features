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
        public SketchPoint ori;
        public KeyValuePair<int, int> ID;
        public int index;
        public double x, y, z;
        public List<int> next;
        public List<KeyValuePair<int, int>> nextSeg;

        public newPoint(SketchPoint poi)
        {
            ori = poi;
            ID = new KeyValuePair<int, int>(poi.GetID()[0], poi.GetID()[1]);
            x = poi.X;
            y = poi.Y;
            z = poi.Z;
            next = new List<int>();
            nextSeg = new List<KeyValuePair<int, int>>();
        }

        public void setIndex(int ind)
        {
            index = ind;
        }

        public void setNext(int nextP)
        {
            next.Add(nextP);
        }

        public void setNextSeg(int segType, int index)
        {
            KeyValuePair<int, int> tem = new KeyValuePair<int, int>(segType, index);
            nextSeg.Add(tem);
        }
    }
}
