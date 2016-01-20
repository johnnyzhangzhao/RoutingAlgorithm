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
        private Dictionary<string,string> parents;

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

        public Dictionary<string, string> DisToNode
        {
            get
            {
                if (parents == null)
                {
                    parents = new Dictionary<string, string>();
                }
                return parents;
            }
        }

        public double GetShortPath(string startNodeId, string targetNodeId)
        {
            visitedNodeMarks = new Dictionary<string, double>();
            double shortestPathCost = 0;
            Collection<Arc> nodeAdjacentArc;
            int numSettledNodes = 0;
            double distToAdjNode = 0;
            ActiveNode startNode;
            ActiveNode activeNode;
            ActiveNode currentNode;
            startNode = new ActiveNode(startNodeId,0,"-1");
            activeNodes = new List<ActiveNode>();
            parents = new Dictionary<string, string>();
            activeNodes.Add(startNode);
            
            
            while (activeNodes.Count() != 0)
            {
                Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                activeNodes.Sort(new ActiveNodeCompare());

                for (int i = 0; i < activeNodes.Count(); i++)
                {
                    Console.WriteLine("pai xu hou"+activeNodes[i].id+","+activeNodes[i].dist);
                }

                currentNode = activeNodes[0];
                Console.WriteLine("***currentnode is"+currentNode.id+"***");

                activeNodes.RemoveAt(0);
                if (isvisited(currentNode.id))
                {
                    continue;
                }
                visitedNodeMarks.Add(currentNode.id, currentNode.dist);
                parents.Add(currentNode.id, currentNode.parent);
                //if (!parents.ContainsKey(currentNode.id))
                //{
                //    parents.Add(currentNode.id, currentNode.parent);
                //}
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
                //Console.WriteLine("nodeAdjacentArc.Count is " + nodeAdjacentArc.Count());
                
                for (int i = 0; i < nodeAdjacentArc.Count(); i++)
                {
                    
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    //Console.WriteLine("arc's node is"+arc.HeadNode.Id+","+arc.TailNode.Id);
                    if (!isvisited(arc.TailNode.Id))
                    { 
                        
                        distToAdjNode = currentNode.dist + nodeAdjacentArc[i].Cost;
                        activeNode = new ActiveNode(arc.TailNode.Id, distToAdjNode,currentNode.id);
                        

                        Console.WriteLine("activenode is "+activeNode.id);
                        activeNodes.Add(activeNode);
                    }
                }
                for (int j = 0; j < activeNodes.Count(); j++)
                {
                    for (int i = activeNodes.Count() - 1; i > j; i--)
                    {
                        if (activeNodes[i].id == activeNodes[j].id && activeNodes[i].dist < activeNodes[j].dist)
                        {
                            activeNodes.RemoveAt(j);
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

        public void ShortPathToString(string startNodeId,string targetNodeId)
        {
            string sourceNodeId = null;
            string path = "";
            Node currentNode = new Node ();
            string currentNodeId = "";
            currentNode = graph.MapNodes[targetNodeId];
            currentNodeId = targetNodeId;
            path = path + currentNode.Id + "->";
            foreach (string t in parents.Keys)
            {
                Console.WriteLine("short path is:" + t + "," + parents[t]);
            }


            
            while (currentNode.Id != sourceNodeId)
            {

                Console.WriteLine("\\\\\\" + currentNode.Id);
                currentNodeId = parents[currentNodeId];
                currentNode = graph.MapNodes[currentNodeId];
                path = path + currentNode.Id + "->";
            }
            Console.WriteLine(path);
        }


    }
}
