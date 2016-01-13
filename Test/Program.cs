using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            RoadNetwork roadNetwork = new RoadNetwork();

            ShapeFileFeatureSource shapeFileFeatureSource = new ShapeFileFeatureSource(@"..\..\Data\roads.shp");
            shapeFileFeatureSource.Open();

            Collection<string> processedList = new Collection<string>();

            Collection<Feature> features = shapeFileFeatureSource.GetAllFeatures(ReturningColumnsType.NoColumns);
            foreach (Feature feature in features)
            {
                Console.WriteLine(string.Format("Start processing feature {0}:"),feature.Id);

                Collection<LineShape> processingLineShapes = GetLineShapes(feature);
                // Get the lineshape of the processing feature.
                foreach (LineShape processingLineShape in processingLineShapes)
                {
                    // Define a variable to save the points where the adjacent lines intersect with current processing line.
                    Collection<PointShape> crossingPoints = new Collection<PointShape>();

                    // Get all the lines in current processing shape bounds.
                    Collection<Feature> adjacentFeatures = shapeFileFeatureSource.GetFeaturesInsideBoundingBox(processingLineShape.GetBoundingBox(), ReturningColumnsType.NoColumns);

                    // Loop and see if the queried shape is intersected with processing shape.
                    foreach (Feature adjacentFeature in adjacentFeatures)
                    {
                        LineBaseShape adjacentLineShape = adjacentFeature.GetShape() as LineBaseShape;
                        MultipointShape tempCrossingPoints = processingLineShape.GetCrossing(adjacentLineShape);

                        // The queried shape is intersected with processing shape.
                        if (tempCrossingPoints.Points.Count > 0)
                        {
                            foreach (PointShape point in tempCrossingPoints.Points)
                            {
                                crossingPoints.Add(point);
                            }
                        }                        
                    }

                    // Order the crossing points following the sequence of line vertex.
                    Collection<PointShape> orderedCrossingPoints = OrderCrossingPoints(processingLineShape, crossingPoints);

                    Node preNode = new Node(roadNetwork.Nodes.Count.ToString(CultureInfo.InvariantCulture), (float)processingLineShape.Vertices[0].Y, (float)processingLineShape.Vertices[0].X);
                    // Split current processing lineshape into arcs and nodes.
                    for (int i = 0; i < orderedCrossingPoints.Count; i++)
                    {
                        // Create the Node and add it into Network
                        Node node = new Node(roadNetwork.Nodes.Count.ToString(CultureInfo.InvariantCulture), (float)orderedCrossingPoints[i].Y, (float)orderedCrossingPoints[i].X);
                        roadNetwork.Nodes.Add(node);

                        // Check if it's start and end point of processing line shape.
                        if (IsSamePoint(orderedCrossingPoints[i], processingLineShape.Vertices[0]) && IsSamePoint(orderedCrossingPoints[i], processingLineShape.Vertices[processingLineShape.Vertices.Count - 1]))
                        {
                            break;
                        }
                        else
                        {
                            // Create the Arc and assign the incomingArcs and outgoingArcs to created Node.
                            //Arc arc = new Arc(roadNetwork.Arcs.Count.ToString(CultureInfo.InvariantCulture),)
                        }
                    }
                }

                // Remove the processed the lineshape to avoid the dupilication.
                processedList.Add(feature.Id);
            }
        }

        private static Collection<PointShape> OrderCrossingPoints(LineShape line, Collection<PointShape> crossingPoints)
        {
            Collection<PointShape> orderedPoints = new Collection<PointShape>();
            for (int i = 0; i < line.Vertices.Count -2; i++)
            {
                Dictionary<PointShape, double> pointsOnSegement = new Dictionary<PointShape,double>();
                foreach (PointShape crossingPoint in crossingPoints)
                {
                    bool isIntermiate = IsIntermediatePoint(line.Vertices[i], line.Vertices[i + 1], crossingPoint);
                    if (isIntermiate)
                    {
                        double pointDistance = GetEvaluatedDistance(line.Vertices[i], crossingPoint);
                        pointsOnSegement.Add(crossingPoint,pointDistance);
                    }
                }

                if (pointsOnSegement.Count > 0)
                {
                    pointsOnSegement.OrderBy(v => v.Value);
                    foreach (var pointOnSegment in pointsOnSegement)
                    {
                        orderedPoints.Add(pointOnSegment.Key);
                        crossingPoints.Remove(pointOnSegment.Key);
                    }
                }
            }

            return orderedPoints;
        }

        private static bool IsIntermediatePoint(Vertex startVetex, Vertex endVertex, PointShape intermeidatePoint)
        {
            return true;
        }

        private static double GetEvaluatedDistance(Vertex point1, PointShape point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        private static bool IsCrossingPoint(Vertex vertex, Collection<PointShape> points)
        {
            foreach (PointShape point in points)
            {
                if (IsSamePoint(point,vertex))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSamePoint(PointShape point1, Vertex point2)
        {
            double tolerance = 1E-06;

            bool isEqual = false;
            if (Math.Abs(point1.X - point2.X) < tolerance && Math.Abs(point1.Y - point2.Y) < tolerance)
            {
                isEqual = true;
            }

            return isEqual;
        }

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
