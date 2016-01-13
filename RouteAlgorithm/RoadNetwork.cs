using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouteAlgorithm
{
    public class RoadNetwork
    {
        private Bounds bounds;
        private List<Arc> arcs;
        private List<Node> nodes;
        private Dictionary<string, Node> mapNodes;

        // Create a adjacent list to store the whole network
        private Dictionary<Node, Collection<Arc>> adjacentArcs;

        public RoadNetwork()
        {
        }

        public Bounds Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }

        public List<Arc> Arcs
        {
            get
            {
                if (arcs == null)
                {
                    arcs = new List<Arc>();
                }
                return arcs;
            }
        }

        public List<Node> Nodes
        {
            get
            {
                if (nodes == null)
                {
                    nodes = new List<Node>();
                }
                return nodes;
            }
        }

        public Dictionary<Node, Collection<Arc>> AdjacentArcs
        {
            get 
            {
                if (adjacentArcs == null)
                {
                    adjacentArcs = new Dictionary<Node, Collection<Arc>>();
                }
                return adjacentArcs; 
            }
        }

        public Dictionary<string, Node> MapNodes
        {
            get
            {
                if (mapNodes == null)
                {
                    mapNodes = new Dictionary<string, Node>();
                }
                return mapNodes;
            }
        }
    }
}
