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


        private Dictionary<Node, double> visitedNodeMarks;

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

        public Dictionary<Node, double> VisitedNodeMarks
        {
            get
            {
                return visitedNodeMarks;
            }

            set
            {
                visitedNodeMarks = value;
            }
        }

        public double GetShortPath(Node startNode, Node targetNode)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();
            //Dictionary<Node, bool> isvisited = new Dictionary<Node, bool>();
            Node currentNode = new Node();
            Node activeNode = new Node();       
            int numSettledNodes = 0;
            Collection<Arc> nodeAdjacentArc = graph.AdjacentArcs[currentNode];
            Collection<Node> path = new Collection<Node>();
            distance.Add(startNode, 0);
            distance.Add(targetNode, -1);
            double mincost = distance[nodeAdjacentArc[0].HeadNode];
            currentNode = startNode;


            double shortestPathCost = 0;
            double distToAdjNode = 0;
            visitedNodeMarks = new Dictionary<Node, double>();
            Queue<Node> pq = new Queue<Node>();
            pq.Enqueue(startNode);
            while(pq.Count()!=0)
            {
                //ActiveNode currentNode=pq.
                //if (isvisited(currentNode))
                //{
                //    continue;
                //}

                //visitedNodeMarks.Add(currentNode, distance[currentNode]);
                //numSettledNodes++;
                //if (currentNode.Id == targetNode.Id)
                //{
                //    shortestPathCost = distance[currentNode];
                //    break;
                //}
                //if ( numSettledNodes > graph.Nodes.Count() )
                //{
                //    shortestPathCost = 99999999999;
                //    break;
                //}
                
                //Collection<Arc> nodeAdjacentArc1 = graph.AdjacentArcs[currentNode];
                //for(int i=0;i<nodeAdjacentArc.Count();i++)
                //{
                //    Arc arc;
                //    arc = nodeAdjacentArc[i];
                //    if (!isvisited(nodeAdjacentArc[i].HeadNode))
                //    {
                //        distToAdjNode = currentNode.+ nodeAdjacentArc[i].Cost;
                //        activeNode=new ActiveNode(arc.HeadNode.Id,) 
                //    }
                //}
            }
            return shortestPathCost;    
        }

        public bool isvisited(Node node)
        {
            if (visitedNodeMarks.ContainsKey(node))
            {
                return true;
            }
            return false;
        }
    }
}
