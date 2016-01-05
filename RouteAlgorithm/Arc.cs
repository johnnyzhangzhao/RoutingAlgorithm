using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class Arc:Coordinate
    {
        public string arcName;
        public string direction;
        public float length;
        public List<Arc> adjacentArcs;
        public List<Node> adjacentNodes;
    }
}
