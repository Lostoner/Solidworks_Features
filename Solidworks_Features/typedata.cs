using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidworks_Features
{
    class typedata
    {
        double Edepth;
        bool EreverseOffset;
        bool EbothDirections;
        double EwallThickness;
        

        public typedata()
        {
            Edepth = 0;
            EreverseOffset = false;
            EbothDirections = false;
            EwallThickness = 0;

        }
    }
}
