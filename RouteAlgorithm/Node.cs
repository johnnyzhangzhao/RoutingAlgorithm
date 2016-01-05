using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class Node:Coordinate
    {
        public List<Arc> intersectArc = new List<Arc>();
        public List<Arc> adjacentArc = new List<Arc>();
        public List<Node> adjacentNode = new List<Node>();

    }
}
