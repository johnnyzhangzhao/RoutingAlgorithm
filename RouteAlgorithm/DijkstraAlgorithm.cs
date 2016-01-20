﻿using System;
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
        private Dictionary<string,double> disToNode;
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

        public Dictionary<string, double> DisToNode
        {
            get
            {
                if (disToNode == null)
                {
                    disToNode = new Dictionary<string, double>();
                }
                return disToNode;
            }
        }

        public double GetShortPath(string startNodeId, string targetNodeId)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();
            visitedNodeMarks = new Dictionary<string, double>();
            double shortestPathCost = 0;
            Collection<Arc> nodeAdjacentArc;
            int numSettledNodes = 0;
            double minCost = 0;
            double distToAdjNode = 0;
            ActiveNode startNode;
            ActiveNode activeNode;
            ActiveNode currentNode;
            startNode = new ActiveNode(startNodeId,0);
            activeNodes = new List<ActiveNode>();
            disToNode = new Dictionary<string, double>();
            activeNodes.Add(startNode);
            disToNode.Add(startNode.id,0);
            
            
            while (activeNodes.Count() != 0)
            {
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@ kaishi@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                activeNodes.Sort(new ActiveNodeCompare());

                currentNode = activeNodes[0];
                minCost = currentNode.Dist;

                if (!disToNode.ContainsKey(currentNode.id))
                {
                    disToNode.Add(currentNode.id, currentNode.Dist);
                }

                
                foreach (string t in disToNode.Keys)
                {
                    Console.WriteLine("short path is:" + t + "," + disToNode[t]);
                }


                activeNodes.RemoveAt(0);
                Console.WriteLine("*************current node is "+currentNode.id+"***********");
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

                        activeNode = new ActiveNode(arc.TailNode.Id, distToAdjNode);

                        Console.WriteLine("activeNode is "+activeNode.id+","+activeNode.Dist);
                        for (int j = 0; j < activeNodes.Count(); j++)
                        {
                            if (activeNode.id == activeNodes[j].id && activeNode.Dist > activeNodes[j].Dist)
                            {
                                DisToNode.Remove(currentNode.id);
                            }
                        }


                        activeNodes.Add(activeNode);  
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

        //public void ShortPathToString()
        //{
        //    foreach (string t in disToNode.Keys)
        //    {
        //        Console.WriteLine("short path is:" + t+","+disToNode[t]);
        //    }
        //}


    }
}
