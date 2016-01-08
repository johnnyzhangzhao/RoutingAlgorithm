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
        private char visible;
        private Dictionary<string,string> tags;
        private string arcName;
        private string direction;
        private float length;
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

        public char Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
            }
        }

        public string ArcName
        {
            get
            {
                return arcName;
            }

            set
            {
                arcName = value;
            }
        }

        public float Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
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
            return "{"+HeadNodeId + "," + ArcName + "," +"}";
        }
    }
}