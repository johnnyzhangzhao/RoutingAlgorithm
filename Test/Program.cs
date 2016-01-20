using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Core;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ShapeFileFeatureSource sourceData = new ShapeFileFeatureSource(@"..\..\Data\Roads.shp");
            StreamSource source = new StreamSource();
            Stopwatch watch = Stopwatch.StartNew();
            RoadNetwork netWork = source.CreateNetwork(sourceData);
            watch.Stop();

            Console.WriteLine(String.Format("The total cost:{0} Minutes, Nodes' count is {1}", watch.ElapsedMilliseconds / 1000 / 60, netWork.Nodes.Count));
            Console.Read();
        }
    }
}
