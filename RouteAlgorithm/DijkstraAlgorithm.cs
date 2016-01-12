using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class DijkstraAlgorithm
    {
        private RoadNetwork graph;
        private Node startNode;
        private Node targetNode;
        private Collection<Node> activeNodes;
        private Collection<Node> settleNodes;
        private Queue<Node> shortNode;

        public DijkstraAlgorithm() { }

        public DijkstraAlgorithm(RoadNetwork graph)
        {
            this.graph = graph;
        }

        public Node StartNode
        {
            set { startNode = value; }
        }

        public Node TargetNode
        {
            set { targetNode = value; }
        }

        public Collection<Node> ActiveNodes
        {
            get
            {
                return activeNodes;
            }

            set
            {
                activeNodes = value;
            }
        }

        public Collection<Node> SettleNodes
        {
            get
            {
                return settleNodes;
            }

            set
            {
                settleNodes = value;
            }
        }

        public Queue<Node> ShortNode
        {
            get
            {
                return shortNode;
            }

            set
            {
                shortNode = value;
            }
        }

        public void GetShortPath(Node startNode, Node targetNode)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();
            Dictionary<Node, bool> isvisited = new Dictionary<Node, bool>();
            Node currentNode = new Node();
            Node activeNode = new Node();
            int numSettledNodes = 0;
            Collection<Arc> nodeAdjacentArc = graph.AdjacentArcs[currentNode];
            Collection<Node> path = new Collection<Node>();
            distance.Add(startNode, 0);
            distance.Add(targetNode, -1);
            double mincost = distance[nodeAdjacentArc[0].HeadNode];
            currentNode = startNode;
            while(currentNode!=targetNode || numSettledNodes<graph.Nodes.Count() )
            {
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    if (distance[currentNode] + nodeAdjacentArc[i].Cost < distance[nodeAdjacentArc[i].HeadNode])
                    {
                        distance[nodeAdjacentArc[i].HeadNode] = distance[currentNode] + nodeAdjacentArc[i].Cost;

                    }

                    if (mincost - distance[nodeAdjacentArc[i].HeadNode] < 0)
                    {
                        mincost = distance[nodeAdjacentArc[i].HeadNode];
                        currentNode = nodeAdjacentArc[i].HeadNode;
                    }
                }
                path.Add(currentNode);
            }        
        }
    }
}
