using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            RoadNetwork roadNetwork = new RoadNetwork();

            ShapeFileFeatureSource shapeFileFeatureSource = new ShapeFileFeatureSource(@"C:\SCNU-R\RouteAlgorithm\Test\Data\roads.shp");
            shapeFileFeatureSource.Open();

            Collection<Feature> features = shapeFileFeatureSource.GetAllFeatures(ReturningColumnsType.NoColumns);
            foreach (Feature feature in features)
            {
                Collection<LineShape> lineshapes = GetLineShapes(feature);


            }

        }

        //private static Node

        private static Collection<LineShape> GetLineShapes(Feature feature)
        {
            Collection<LineShape> lineShapes = new Collection<LineShape>();

            LineBaseShape lineBaseShape = feature.GetShape() as LineBaseShape;
            if (lineBaseShape is MultilineShape)
            {
                MultilineShape multiLineShape = lineBaseShape as MultilineShape;
                lineShapes = multiLineShape.Lines;
            }
            else if (lineBaseShape is LineShape)
            {
                lineShapes.Add((LineShape)lineBaseShape);
            }
            else 
            {
                throw new NotSupportedException("Only the data which includes the line shapes is supported.");
            }

            return lineShapes;
        }
    }
}
