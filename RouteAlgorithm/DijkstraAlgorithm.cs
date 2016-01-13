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


        private Dictionary<string, double> visitedNodeMarks;

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

        public Dictionary<string, double> VisitedNodeMarks
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
            //Node currentNode = new Node();
            //Node activeNode = new Node();       
            int numSettledNodes = 0;
            
            Collection<Node> path = new Collection<Node>();
            //distance.Add(startNode, 0);
            distance.Add(targetNode, -1);
            


            double shortestPathCost = 0;
            double distToAdjNode = 0;
            visitedNodeMarks = new Dictionary<string, double>();
            
            ActiveNode activeNode=new ActiveNode (startNode.Id,0);
            Queue<ActiveNode> pq = new Queue<ActiveNode>();
            pq.Enqueue(activeNode);
            
            while(pq.Count()!=0)
            {
                ActiveNode currentNode = pq.Dequeue();
                if (isvisited(currentNode.id))
                {
                    continue;
                }

                visitedNodeMarks.Add(currentNode.id, currentNode.dist);
                numSettledNodes++;
                if (currentNode.id == targetNode.Id)
                {
                    shortestPathCost = currentNode.dist;
                    break;
                }
                if (numSettledNodes > graph.Nodes.Count())
                {
                    shortestPathCost = 99999999999;
                    break;
                }

                
                Node curNode;
                curNode = graph.MapNodes[currentNode.id];
                Collection<Arc> nodeAdjacentArc = graph.AdjacentArcs[curNode];
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    double minCost = 0;
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    if (!isvisited(arc.HeadNode.Id))
                    {
                        distToAdjNode = currentNode.dist+ nodeAdjacentArc[i].Cost;
                        activeNode = new ActiveNode(arc.HeadNode.Id, distToAdjNode);
                    }
                    if (activeNode.dist < minCost)
                    {
                        pq.Enqueue(activeNode);
                    }
                }
            }
            return shortestPathCost;    
        }

        public bool isvisited(string nodeId)
        {
            if (visitedNodeMarks.ContainsKey(nodeId))
            {
                return true;
            }
            return false;
        }
    }
}
