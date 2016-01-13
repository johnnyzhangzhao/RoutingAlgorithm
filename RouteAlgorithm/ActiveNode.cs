using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class ActiveNode
    {
        private string id;
        private double dist;
        public ActiveNode(string id, double dist)
        {
            this.id = id;
            this.dist = dist;
        }
    }
}
