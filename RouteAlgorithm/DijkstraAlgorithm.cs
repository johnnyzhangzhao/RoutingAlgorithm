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
        private List<ActiveNode> activeNodes;
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

        public List<ActiveNode> ActiveNodes
        {
            get
            {
                if (activeNodes == null)
                {
                    activeNodes = new List<ActiveNode>();
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
            ActiveNode currentNode;
            startNode = new ActiveNode(startNodeId,0);
            activeNodes = new List<ActiveNode>();
            activeNodes.Add(startNode);
            
            
            while (activeNodes.Count() != 0)
            {

                activeNodes.Sort(new ActiveNodeCompare());

                for (int i = 0; i < activeNodes.Count(); i++)
                {
                    Console.WriteLine("pai xu hou "+activeNodes[i].id);
                }

                currentNode = activeNodes[0];
                activeNodes.Clear();
                Console.WriteLine("From Node: " + startNodeId + " to Node " + targetNodeId);
                Console.WriteLine("****************current node is "+currentNode.id+"***********");
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
                
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    
                    //minCost = activeNode.dist;
                    if (!isvisited(arc.TailNode.Id))
                    { 
                        distToAdjNode = currentNode.Dist + nodeAdjacentArc[i].Cost;
                        Console.WriteLine("distToAdjNode is " + distToAdjNode);
                        activeNode = new ActiveNode(arc.TailNode.Id, distToAdjNode);

                        Console.WriteLine("activeNode is "+activeNode.id);

                        activeNodes.Add(activeNode);
                        Console.WriteLine(activeNodes.Count());
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
