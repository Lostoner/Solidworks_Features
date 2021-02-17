using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class loopSeg
    {
        public SketchSegment seg;       //边的源数据
        public int type;                         //边的类型
        public int index;                       //边的索引（目前无用）
        public int start;                        //边的起始点索引
        public int end;                         //边的终结点索引

        public void setIndex(int num)
        {
            index = num;
        }

        public void setPoint(int sta, int en)
        {
            start = sta;
            end = en;
        }

        public loopSeg(SketchSegment iniSeg)
        {
            seg = iniSeg;
            type = seg.GetType();
            index = -1;
            start = -1;
            end = -1;
        }

        public bool same(loopSeg tem)
        {
            if(seg == tem.seg)
            {
                return true;
            }
            else if((start == tem.start && end == tem.end) || (start == tem.end && end == tem.start))
            {
                if(seg.GetType() == tem.seg.GetType())
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
