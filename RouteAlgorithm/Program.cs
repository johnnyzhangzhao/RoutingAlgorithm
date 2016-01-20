using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Node n4 = new Node("4", 0, 0);
            Node n5 = new Node("5", 0, 0);
            Node n6 = new Node("6", 0, 0);

            Arc arc12 = new Arc(n1, n2, 7);
            Arc arc13 = new Arc(n1, n3, 9);
            Arc arc16 = new Arc(n1, n6, 14);
            Arc arc23 = new Arc(n2, n3, 10);
            Arc arc24 = new Arc(n2, n4, 15);
            Arc arc34 = new Arc(n3, n4, 11);
            Arc arc36 = new Arc(n3, n6, 2);
            Arc arc45 = new Arc(n4, n5, 6);
            Arc arc65 = new Arc(n6, n5, 9);

            Arc arc21 = new Arc(n2, n1, 7);
            Arc arc31 = new Arc(n3, n1, 9);
            Arc arc61 = new Arc(n6, n1, 14);
            Arc arc32 = new Arc(n3, n2, 10);
            Arc arc42 = new Arc(n4, n2, 15);
            Arc arc43 = new Arc(n4, n3, 11);
            Arc arc63 = new Arc(n6, n3, 2);
            Arc arc54 = new Arc(n5, n4, 6);
            Arc arc56 = new Arc(n6, n5, 9);

            rn.Nodes.Add(n1);
            rn.Nodes.Add(n2);
            rn.Nodes.Add(n3);
            rn.Nodes.Add(n4);
            rn.Nodes.Add(n5);
            rn.Nodes.Add(n6);
            rn.MapNodes.Add("1", n1);
            rn.MapNodes.Add("2", n2);
            rn.Arcs.Add(arc12);
            rn.Arcs.Add(arc13);
            rn.Arcs.Add(arc16);
            rn.AdjacentArcs.Add(n1.Id, rn.Arcs);

            
            Collection<Arc> aa = new Collection<Arc>();
            aa.Add(arc21);
            aa.Add(arc23);
            aa.Add(arc24);
            rn.AdjacentArcs.Add(n2.Id,aa);

            Collection<Arc> bb = new Collection<Arc>();
            bb.Add(arc31);
            bb.Add(arc32);
            bb.Add(arc34);
            bb.Add(arc36);
            rn.AdjacentArcs.Add(n3.Id, bb);

            Collection<Arc> cc = new Collection<Arc>();
            cc.Add(arc42);
            cc.Add(arc43);
            cc.Add(arc45);
            rn.AdjacentArcs.Add(n4.Id, cc);

            Collection<Arc> dd = new Collection<Arc>();
            dd.Add(arc63);
            dd.Add(arc65);
            dd.Add(arc61);
            rn.AdjacentArcs.Add(n6.Id, dd);

            Collection<Arc> ee = new Collection<Arc>();
            ee.Add(arc56);
            ee.Add(arc54);
            rn.AdjacentArcs.Add(n5.Id, ee);

            DijkstraAlgorithm dij = new DijkstraAlgorithm(rn );
            double cost = dij.GetShortPath("1", "5");
            //dij.ShortPathToString();
            Console.WriteLine("min cost:"+cost);
            Console.ReadLine();
        }
    }
}
