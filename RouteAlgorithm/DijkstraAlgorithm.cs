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
        private Dictionary<string, double> visitedNodeMarks;
        private Collection<ActiveNode> activeNodes;
        private Queue<ActiveNode> shortNode;

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

        public Dictionary<string, double> VisitedNodeMarks
        {
            get
            {
                if (visitedNodeMarks == null)
                {
                    visitedNodeMarks = new Dictionary<string, double>();
                }
                return visitedNodeMarks;
            }
        }

        public Collection<ActiveNode> ActiveNodes
        {
            get
            {
                if (activeNodes == null)
                {
                    activeNodes = new Collection<ActiveNode>();
                }
                return activeNodes;
            }
        }

        public Queue<ActiveNode> ShortNode
        {
            get
            {
                if (shortNode == null)
                {
                    shortNode = new Queue<ActiveNode>();
                }
                return shortNode;
            }
        }

        public void ShortPathToString()
        {
            foreach (ActiveNode n1 in ShortNode)
            {
                Console.Write("shortNode:{0}", n1.id);
            }
        }

        public double GetShortPath(string startNodeId, string targetNodeId)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();
            visitedNodeMarks = new Dictionary<string, double>();
            double shortestPathCost = 0;
            Collection<Arc> nodeAdjacentArc;
            int numSettledNodes = 0;
            double distToAdjNode = 0;
            ActiveNode startNode;
            ActiveNode activeNode;
            startNode = new ActiveNode(startNodeId,0);
            Queue<ActiveNode> pq = new Queue<ActiveNode>();
            pq.Enqueue(startNode);
            while (pq.Count() != 0)
            {
                ActiveNodes.Add(pq.Dequeue());
                ActiveNode currentNode = ActiveNodes[activeNodes.Count()-1];
                if (isvisited(currentNode.id))
                {
                    continue;
                }
                visitedNodeMarks.Add(currentNode.id, currentNode.dist);
                numSettledNodes++;
                if (currentNode.id == targetNodeId)
                {
                    shortestPathCost = currentNode.dist;
                    break;
                }
                if (numSettledNodes > graph.Nodes.Count())
                {
                    Console.WriteLine("There is no short path between startNode and targetNode");
                    break;
                }
                nodeAdjacentArc = this.graph.AdjacentArcs[currentNode.id];
                double minCost=0 ;
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    
                    //minCost = activeNode.dist;
                    if (!isvisited(arc.HeadNode.Id))
                    {
                        
                        distToAdjNode = currentNode.dist + nodeAdjacentArc[i].Cost;
                        activeNode = new ActiveNode(arc.HeadNode.Id, distToAdjNode);
                        if (minCost==0)
                        {
                            minCost = activeNode.dist;
                            pq.Enqueue(activeNode);
                        }
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
