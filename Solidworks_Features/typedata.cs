using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class typedata
    {
        public int type;

        public double Edepth;
        public bool EreverseOffset;
        public bool EbothDirections;
        public double EwallThickness;

        public double LstartTangentLength;
        public double LendTangentLength;
        public double LwallThickness;
        //public List<double> LendDirectionVector;
        //public List<double> LstartDirectionVector;

        public double BtangentLength;
        //public double BwallThickness1;
        //public double BwallThickness2;
        public double BwallThickness;

        public double Rangle;
        public double RwallThickness;
        //旋转轴的提取还未完成

        public double SwallThickness;
        //扫描曲线的提取还未完成

        public typedata(int oriType)
        {
            type = oriType;

            Edepth = 0;
            EreverseOffset = false;
            EbothDirections = false;
            EwallThickness = 0;
        }
    }
}
