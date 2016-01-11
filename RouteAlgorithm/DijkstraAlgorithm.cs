using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class DijkstraAlgorithm
    {
        private Node startNode;
        private Node targetNode;
        private Collection<Node> activeNode;
        private Collection<Node> settleNode;
        private Queue<Node> shortNode;

        public DijkstraAlgorithm() { }

        public DijkstraAlgorithm(Node startNode,Node targetNode)
        {
            this.StartNode = startNode;
            this.TargetNode = targetNode;
            
        }


        public Node StartNode
        {
            set  {  startNode = value; }
        }

        public Node TargetNode
        {
            set { targetNode = value; }
        }

        public Collection<Node> ActiveNode
        {
            get
            {
                return activeNode;
            }

            set
            {
                activeNode = value;
            }
        }

        public Collection<Node> SettleNode
        {
            get
            {
                return settleNode;
            }

            set
            {
                settleNode = value;
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

        public void GetShortPath(Node startNode,Node targetNode)
        {
            double minDistance=0;

        }
        public void RelaxOutgoingArcs()
        {

        }
    }
}
