using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    public class LandmarkAlgorithm
    {
        private RoadNetwork graph;
        private DijkstraAlgorithm dijkstra;
        private List<double> landmarks;
        int numOfLandmarks;

        public LandmarkAlgorithm() { }

        public LandmarkAlgorithm(RoadNetwork graph)
        {
            this.graph = graph;
        }

        public void SelectLandmarks(int numOfLandmarks)
        {
            if (numOfLandmarks < graph.Nodes.Count())
            {
                this.numOfLandmarks = numOfLandmarks;
                for (int i=0;i<numOfLandmarks;i++)
                {

                }
            }
        }

        public void ComputLandmarks()
        {

        }

        

        public void ComputShortPath()
        { }
    }
}
