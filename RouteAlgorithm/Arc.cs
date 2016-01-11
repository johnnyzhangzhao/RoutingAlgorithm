using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class Arc
    {
        private int headNodeId;
        private int cost;
        private Dictionary<string,string> tags;
        private List<Node> intersectNode;
        private List<Arc> adjacentArcs;
        private List<Node> adjacentNodes;

        public Arc() { }

        public Arc(int headNodeId, int cost)
        {
            this.headNodeId = headNodeId;
            this.Cost = cost;
        }

        public int HeadNodeId
        {
            get
            {
                return headNodeId;
            }

            set
            {
                headNodeId = value;
            }
        }

        public int Cost
        {
            get
            {
                return cost;
            }

            set
            {
                cost = value;
            }
        }

        public Dictionary<string, string> Tags
        {
            get
            {
                if (tags == null)
                {
                    tags = new Dictionary<string, string>();
                }
                return tags;
            }
        }

        public override string ToString()
        {
            return "{"+HeadNodeId + "," + cost  +"}";
        }
    }
}