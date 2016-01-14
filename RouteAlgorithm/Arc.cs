using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouteAlgorithm
{
    public class Arc
    {
        private string id;
        private float cost;
        private Node tailNode;
        private Node headNode;
        private Collection<Node> intermediateNodes;
        private RoadDirection direction;
        private string name;
        private Dictionary<string, string> tags;

        private Collection<Arc> incomingArcs;
        private Collection<Arc> outgoingArcs;

        public Arc()
        {
        }

        public Arc(Node headNode, Node tailNode, float cost)
            :this(string.Empty,headNode,tailNode,cost)
        {
        }

        public Arc(string id, Node headNode, Node tailNode, float cost)
        {
            this.id = id;
            this.Cost = cost;
            this.HeadNode = headNode;
            this.TailNode = tailNode;
        }

        public string Id
        {
            get { return id; }
            internal set { id = value; }
        }

        public float Cost
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

        public Node HeadNode
        {
            get { return headNode; }
            set { headNode = value; }
        }

        public Node TailNode
        {
            get { return tailNode; }
            set { tailNode = value; }
        }

        public Collection<Node> IntermediateNodes
        {
            get
            {
                if (intermediateNodes == null)
                {
                    intermediateNodes = new Collection<Node>();
                }
                return intermediateNodes;
            }
        }

        public RoadDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
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

        public Collection<Arc> IncomingArcs
        {
            get
            {
                if (incomingArcs == null)
                {
                    incomingArcs = new Collection<Arc>();
                }
                return incomingArcs;
            }
        }

        public Collection<Arc> OutgoingArcs
        {
            get
            {
                if (outgoingArcs == null)
                {
                    outgoingArcs = new Collection<Arc>();
                }
                return outgoingArcs;
            }
        }

        public override string ToString()
        {
            return string.Format("headNode:{0},tailNode:{1},cost:{2}", headNode,tailNode,cost);
        }
    }
}