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
            Node n7 = new Node("7", 0, 0);
            Node n8 = new Node("8", 0, 0);
            Node n9 = new Node("9", 0, 0);

            Arc arc12 = new Arc(n1, n2, 2);
            Arc arc16 = new Arc(n1, n6, 1);

            Arc arc21 = new Arc(n2, n1, 2);
            Arc arc23 = new Arc(n2, n3, 2);

            Arc arc32 = new Arc(n3, n2, 2);
            Arc arc34 = new Arc(n3, n4, 2);

            Arc arc43 = new Arc(n4, n3, 2);
            Arc arc45 = new Arc(n4, n5, 2);

            Arc arc54 = new Arc(n5, n4, 2);
            Arc arc57 = new Arc(n5, n7, 10);
            Arc arc58 = new Arc(n5, n8, 10);
            Arc arc59 = new Arc(n5, n9, 10);

            Arc arc61 = new Arc(n6, n1, 1);
            Arc arc67 = new Arc(n6, n7, 1);
            Arc arc68 = new Arc(n6, n8, 1);
            Arc arc69 = new Arc(n6, n9, 1);

            Arc arc76 = new Arc(n7, n6, 1);
            Arc arc75 = new Arc(n7, n5, 10);

            Arc arc86 = new Arc(n8, n6, 1);
            Arc arc85 = new Arc(n8, n5, 10);

            Arc arc96 = new Arc(n9, n6, 1);
            Arc arc95 = new Arc(n9, n5, 10);

            rn.Nodes.Add(n1);
            rn.Nodes.Add(n2);
            rn.Nodes.Add(n3);
            rn.Nodes.Add(n4);
            rn.Nodes.Add(n5);
            rn.Nodes.Add(n6);
            rn.Nodes.Add(n7);
            rn.Nodes.Add(n8);
            rn.Nodes.Add(n9);

            rn.MapNodes.Add("1",n1);
            rn.MapNodes.Add("2", n2);
            rn.MapNodes.Add("3", n3);
            rn.MapNodes.Add("4", n4);
            rn.MapNodes.Add("5", n5);
            rn.MapNodes.Add("6", n6);
            rn.MapNodes.Add("7", n7);
            rn.MapNodes.Add("8", n8);
            rn.MapNodes.Add("9", n9);

            Collection<Arc> aa = new Collection<Arc>();
            aa.Add(arc12);
            aa.Add(arc16);
            rn.AdjacentArcs.Add(n1.Id, aa);
            Collection<Arc> bb = new Collection<Arc>();
            bb.Add(arc21);
            bb.Add(arc23);
            rn.AdjacentArcs.Add(n2.Id, bb);

            Collection<Arc> cc = new Collection<Arc>();
            cc.Add(arc32);
            cc.Add(arc34);
            rn.AdjacentArcs.Add(n3.Id, cc);

            Collection<Arc> dd = new Collection<Arc>();
            dd.Add(arc43);
            dd.Add(arc45);
            rn.AdjacentArcs.Add(n4.Id, dd);
            Collection<Arc> ee = new Collection<Arc>();
            ee.Add(arc57);
            ee.Add(arc54);
            ee.Add(arc58);
            ee.Add(arc59);
            rn.AdjacentArcs.Add(n5.Id, ee);

            Collection<Arc> ff = new Collection<Arc>();
            ff.Add(arc67);
            ff.Add(arc68);
            ff.Add(arc61);
            ff.Add(arc69);
            rn.AdjacentArcs.Add(n6.Id, ff);
            Collection<Arc> gg = new Collection<Arc>();
            gg.Add(arc75);
            gg.Add(arc76);
            rn.AdjacentArcs.Add(n7.Id, gg);
            Collection<Arc> hh = new Collection<Arc>();
            hh.Add(arc85);
            hh.Add(arc86);
            rn.AdjacentArcs.Add(n8.Id, hh);
            Collection<Arc> tt = new Collection<Arc>();
            tt.Add(arc95);
            tt.Add(arc96);
            rn.AdjacentArcs.Add(n9.Id, tt);

            DijkstraAlgorithm dij = new DijkstraAlgorithm(rn);
            double cost = dij.GetShortPath("1", "5");
            Console.WriteLine("cost is:" + cost);
            dij.ShortPathToString("1", "5");

            Console.ReadLine();
        }
    }
}
