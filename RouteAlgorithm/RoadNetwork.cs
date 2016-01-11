using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteAlgorithm
{
    public class RoadNetwork
    {
        private Bounds bounds;
        private List<Arc> arcs;
        private List<Node> nodes;
        private List<List<Arc>> adjacentArc;

        public RoadNetwork()
        {
        }

        internal Bounds Bounds
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

        public List<List<Arc>> AdjacentArc
        {
            get
            {
                if (adjacentArc == null)
                {
                    adjacentArc = new List<List<Arc>>();
                }
                return adjacentArc;
            }            
        }

        public string GetDirection(Node startNode, Node endNode, Arc road)
        {
            if (road.Tags.ContainsKey(dirction)==1)
            {

            }
          throw new NotImplementedException() ;
        }

        public void readFromOsmFile(string fileName)
        {
            
        }

        public override string ToString()
        {
            return "this road network has"+nodes.Count()+"nodes"+arcs.Count()+"arcs";
        }
    }
}
