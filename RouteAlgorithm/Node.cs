using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class Node
    {
        private string nodeName;
        private string traffic_sign;
        private string city_limit;
        private List<Arc> intersectArc ;
        private List<Arc> adjacentArc;
        private List<Node> adjacentNode ;

        public Node() { }

        public Node(int id, float latitude, float longitude)
        {
            Coordinate co = new Coordinate();
            co.Id = id;
            co.Latitude = latitude;
            co.Longitude = longitude;
        }

        public List<Arc> IntersectArc
        {
            get
            {
                return intersectArc;
            }

            set
            {
                intersectArc = value;
            }
        }

        public List<Arc> AdjacentArc
        {
            get
            {
                return adjacentArc;
            }

            set
            {
                adjacentArc = value;
            }
        }

        public List<Node> AdjacentNode
        {
            get
            {
                return adjacentNode;
            }

            set
            {
                adjacentNode = value;
            }
        }

        public override string ToString()
        {
            return "{";
        }
    }
}
