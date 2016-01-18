using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class ActiveNode
    {
        public string id;
        private double dist;

        public ActiveNode() { }

        public ActiveNode(string id, double dist)
        {
            this.id = id;
            this.Dist = dist;
        }

        public double Dist
        {
            get
            {
                return dist;
            }

            set
            {
                dist = value;
            }
        }
    }
}
