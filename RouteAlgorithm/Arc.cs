using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouteAlgorithm
{
    public class Arc
    {
        private string id;
        private Coordinate coordinate;
        private double cost;
        private Node tailNode;
        private Node headNode;
        private double length;
        private RoadSpeed speed;
        private Collection<Node> intermediateNodes;
        private RoadDirection direction;
        private string name;
        private Dictionary<string, string> tags;

        private Collection<Arc> incomingArcs;
        private Collection<Arc> outgoingArcs;

        public Arc()
        {
        }

        public Arc(string id, Node headNode, Node tailNode, float cost)
        {
            this.Cost = cost;
        }

        public string Id
        {
            get { return id; }
            private set { id = value; }
        }

        public double Cost
        {
            get { return cost; }
            set { cost = value; }
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


        public double Length
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


        public RoadSpeed Speed
        {
            get { return speed; }
            set { speed = value; }
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
            return string.Format("id:{0},headNode:{3},tailNode:{4},cost:{5}", id, headNode, tailNode, cost);
        }
    }
}