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

        public double CompareCost(ActiveNode n1, ActiveNode n2)
        {
            double dist = n1.dist - n2.dist;
            if (dist < 0)
            {
                return n1.dist;
            }
            return n2.dist;
        }

        public double GetShortPath(Node startNode, Node targetNode)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();




            visitedNodeMarks = new Dictionary<string, double>();
            double shortestPathCost = 0;
            int numSettledNodes = 0;
            double distToAdjNode = 0;
            ActiveNode activeNode = new ActiveNode(startNode.Id, 0);
            Queue<ActiveNode> pq = new Queue<ActiveNode>(1);
            pq.Enqueue(activeNode);

            while (pq.Count() != 0)
            {
                ActiveNodes.Add(pq.Dequeue());
                ActiveNode currentNode = ActiveNodes[activeNodes.Count()];
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
                    Arc arc;
                    arc = nodeAdjacentArc[i];
                    double minCost = 0;
                    minCost = activeNode.dist;
                    if (!isvisited(arc.HeadNode.Id))
                    {
                        distToAdjNode = currentNode.dist + nodeAdjacentArc[i].Cost;
                        shortNode.Enqueue(currentNode);
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

        public void ShortPathToString()
        {
            foreach (ActiveNode n1 in ShortNode)
            {
                Console.Write ("shortNode:{0}",n1.id);
            }
        }
    }
}
