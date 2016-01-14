using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm.Tests
{
    [TestClass()]
    public class DijkstraAlgorithmTests
    {
        public RoadNetwork buildGraph()
        {
            RoadNetwork rn = new RoadNetwork();
            Node n1 = new Node("1", 0, 0);
            Node n2 = new Node("2", 0, 0);
            //Node n3 = new Node("3", 0, 0);
            Node n4 = new Node("4", 0, 0);
            //Node n5 = new Node("5", 0, 0);
            //Node n6 = new Node("6", 0, 0);

            Arc arc1 = new Arc(n1, n2, 7);
            //Arc arc2 = new Arc(n1, n3, 9);
            //Arc arc3 = new Arc(n1, n6, 14);
            //Arc arc4 = new Arc(n6, n5, 9);
            //Arc arc5 = new Arc(n3, n4, 11);
            Arc arc6 = new Arc(n2, n4, 15);
            //Arc arc7 = new Arc(n6, n3, 2);
            //Arc arc8 = new Arc(n3, n2, 10);
            //Arc arc9 = new Arc(n5, n4, 6);

            rn.Nodes.Add(n1);
            rn.Nodes.Add(n2);
            //rn.Nodes.Add(n3);
            rn.Nodes.Add(n4);
            //rn.Nodes.Add(n5);
            //rn.Nodes.Add(n6);

            rn.MapNodes.Add("1", n1);
            rn.MapNodes.Add("2", n2);
            //rn.MapNodes.Add("3", n3);
            rn.MapNodes.Add("4", n4);
            //rn.MapNodes.Add("5", n5);
            //rn.MapNodes.Add("6", n6);

            rn.Arcs.Add(arc1);
            //rn.Arcs.Add(arc2);
            //rn.Arcs.Add(arc3);


            rn.AdjacentArcs.Add(n1.Id, rn.Arcs);
            //rn.Arcs.Clear();

            //rn.Arcs.Add(arc5);
            rn.Arcs.Add(arc6);

            rn.AdjacentArcs.Add(n2.Id, rn.Arcs);
            return rn;
        }

        //test getshortpath 
        [TestMethod()]
        public void GetShortPathTest()
        {
            RoadNetwork rn = buildGraph();
            DijkstraAlgorithm dij = new DijkstraAlgorithm(rn);
            Node n1 = new Node("1", 0, 0);

            Node n2 = new Node("2", 0, 0);
            Node n4 = new Node("4", 0, 0);
            double cost = dij.GetShortPath(n2, n4);
            double exceptde = 7;
            Assert.AreEqual(cost, exceptde);
        }
    }
}