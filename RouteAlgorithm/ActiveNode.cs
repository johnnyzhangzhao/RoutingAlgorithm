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
        public double heuristic;
        public string parent;

        public ActiveNode() { }

        public ActiveNode(string id, double dist, double heuristic, string parent)
        {
            this.id = id;
            this.dist = dist;
            this.heuristic=heuristic;
            this.parent = parent;
        }
    }
}
