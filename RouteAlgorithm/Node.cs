using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouteAlgorithm
{
    public class Node
    {
        private string id;
        public Coordinate coordinate;      
        private Dictionary<string, string> tags;
        private Collection<Arc> incomingArcs;
        private Collection<Arc> outgoingArcs;

        public Node() 
        { 
        }

        public Node(string id, float latitude, float longitude)
        {
            this.id = id;
            this.Coordinate = new Coordinate(latitude, longitude);
        }

        public string Id
        {
            get { return id; }
            internal set { id = value; }
        }

        public Coordinate Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
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
            return string.Format("id:{0}", id); 
        }
    }
}
