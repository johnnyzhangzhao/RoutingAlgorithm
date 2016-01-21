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
        public string parent;

        public ActiveNode() { }

        public ActiveNode(string id, double dist,string parent)
        {
            this.id = id;
            this.dist = dist;
            this.parent = parent;
        }
    }
}
