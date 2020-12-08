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

        public double ox, oy, oz;

        public List<int> nextSegs;

        public newPoint(SketchPoint poi)
        {
            ori = poi;
            ID = new KeyValuePair<int, int>(poi.GetID()[0], poi.GetID()[1]);
            x = poi.X;
            y = poi.Y;
            z = poi.Z;

            ox = 0;
            oy = 0;
            oz = 0;

            next = new List<int>();
            nextSeg = new List<KeyValuePair<int, int>>();

            nextSegs = new List<int>();
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

        public void setNextSeg2(int index)
        {
            nextSegs.Add(index);
        }

        public void setLocation(double x, double y, double z)
        {
            ox = x;
            oy = y;
            oz = z;
        }
    }
}
