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

        public ActiveNode CostCompare(ActiveNode n1, ActiveNode n2)
        {
            double dist = (n1.Dist) - (n2.Dist);
            if (dist < 0)
            {
                return n1;
            }
            else
            {
                return n2;
            }
        }

        //public ActiveNode getShortNode()
        //{
        //    ActiveNode minNode
        //}

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
            ActiveNode midNode;
            double minCost;
            startNode = new ActiveNode(startNodeId,0);
            //currentNode = startNode;
            
            
            while (activeNodes.Count() != 0)
            {
                
                //midNode = pq.Dequeue();
                //activeNodes.Add(midNode);
                Console.WriteLine(activeNodes.Count());
               // activeNodes.Add(pq.Dequeue());
                ActiveNode currentNode = activeNodes[activeNodes.Count() - 1];

                Console.WriteLine("From Node: " + startNodeId + " to Node " + targetNodeId);
                if (isvisited(currentNode.id))
                {
                    continue;
                }
                visitedNodeMarks.Add(currentNode.id, currentNode.Dist);
                numSettledNodes++;

                Console.WriteLine(numSettledNodes);

                if (currentNode.id == targetNodeId)
                {
                    shortestPathCost = currentNode.Dist;
                    break;
                }
                if (numSettledNodes > graph.Nodes.Count())
                {
                    Console.WriteLine("There is no short path between startNode and targetNode");
                    break;
                }
                nodeAdjacentArc = this.graph.AdjacentArcs[currentNode.id];

                
                minCost= currentNode.Dist + nodeAdjacentArc[0].Cost;
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    
                    //minCost = activeNode.dist;
                    if (!isvisited(arc.HeadNode.Id))
                    { 
                        distToAdjNode = currentNode.Dist + nodeAdjacentArc[i].Cost;
                        distToAdjNode = 22222;
                        Console.WriteLine(distToAdjNode);
                        activeNode = new ActiveNode(arc.HeadNode.Id, distToAdjNode);
                        if (activeNode.Dist-minCost<0)
                        {
                            minCost = activeNode.Dist;
                            currentNode = activeNode;
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

        public void ShortPathToString()
        {
            foreach (ActiveNode n1 in ShortNode)
            {
                Console.Write("shortNode:{0}", n1.id);
            }
        }


    }
}
