using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteAlgorithm
{
    public class RoadNetwork
    {
        public float coverager;
        public int numNodes;
        public int numArcs;
        public Arc mapArcs = new Arc();
       
        public List<Arc> segments=new List<Arc> ();
        public List<Node> nodes =new List<Node> ();

        public void AddNode()
        { }
        public void AddArc()
        { }
        public void GetDirection(Node startNode,Node endNode)
        { }
        public void showRoadNetwork()
        {
            
        }

    }
}
