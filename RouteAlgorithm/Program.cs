using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    class Program
    {

        static void Main(string[] args)
        {
            RoadNetwork rn = new RoadNetwork();
            Node n1 = new Node("1", 0, 0);
            Node n2 = new Node("2", 0, 0);
            Node n3 = new Node("3", 0, 0);
            Arc arc1 = new Arc(n1, n2, 7);
            Arc arc2 = new Arc(n2, n3, 3);
            rn.Nodes.Add(n1);
            rn.Nodes.Add(n2);
            rn.Nodes.Add(n3);
            rn.MapNodes.Add("1", n1);
            rn.MapNodes.Add("2", n2);
            rn.Arcs.Add(arc1);
            rn.Arcs.Add(arc2);
            rn.AdjacentArcs.Add(n1.Id, rn.Arcs);
            rn.AdjacentArcs.Add(n2.Id,rn.Arcs);
            DijkstraAlgorithm dij = new DijkstraAlgorithm(rn );
            double cost = dij.GetShortPath("1", "3");
            dij.ShortPathToString();
            Console.WriteLine("min cost:"+cost);
            Console.ReadLine();
        }
    }
}
