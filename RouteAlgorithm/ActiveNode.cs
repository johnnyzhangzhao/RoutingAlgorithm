using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class ActiveNode
    {
        public string id;
        public double dist;
        public ActiveNode(string id, double dist)
        {
            this.id = id;
            this.dist = dist;
        }
    }
}
