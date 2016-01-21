using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RouteAlgorithm
{
    public class RoadNetwork
    {
        private Bounds bounds;
        private Collection<Arc> arcs;
        private List<Node> nodes;
        private Dictionary<string, Node> mapNodes;

        // Create a adjacent list to store the whole network
        private Dictionary<string, Collection<Arc>> adjacentArcs;

        public RoadNetwork() { }

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

        public Collection<Arc> Arcs
        {
            get
            {
                if (arcs == null)
                {
                    arcs = new Collection<Arc>();
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

        public Dictionary<string, Collection<Arc>> AdjacentArcs
        {
            get 
            {
                if (adjacentArcs == null)
                {
                    adjacentArcs = new Dictionary<string, Collection<Arc>>();
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

        public double ComputeDistance(Node n1, Node n2)
        { 
            double midLat = n1.coordinate.Latitude - n2.coordinate.Longitude;
            double midLng = n1.coordinate.Longitude - n2.coordinate.Longitude;
            double dist = Math.Sqrt(Math.Pow(midLat,2)+Math.Pow(midLng,2));
            return dist;
        }
    }
}
