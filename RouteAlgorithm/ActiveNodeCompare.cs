using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class ActiveNodeCompare:IComparer<ActiveNode>
    {
        public int Compare(ActiveNode n1, ActiveNode n2)
        {
            double dist = n1.Dist - n2.Dist;
            if (dist < 0)
            {
                return -1;
            }
            return 1;
        }
    }
}
