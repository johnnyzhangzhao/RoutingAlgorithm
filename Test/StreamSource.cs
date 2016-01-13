using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ThinkGeo.MapSuite.Core;

namespace Test
{
    public class StreamSource
    {
        private double tolerance;

        public StreamSource()
        {
            tolerance = 1e-6;
        }

        public double Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }

        public virtual RoadNetwork CreateNetwork(FeatureSource featureSource)
        {
            RoadNetwork roadNetwork = new RoadNetwork();

            featureSource.Open();
            Collection<Feature> features = featureSource.GetAllFeatures(ReturningColumnsType.NoColumns);
            foreach (Feature feature in features)
            {
                Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
                // Get the lineshape of the processing feature.
                foreach (LineShape processingLineShape in processingLineShapes)
                {
                    Vertex startVertex = processingLineShape.Vertices[0];

                    // Create node based on vertex.
                    Node node = new Node(roadNetwork.Nodes.Count.ToString(CultureInfo.InvariantCulture), (float)startVertex.X, (float)startVertex.Y);

                    RectangleShape startSmallBounds = GeometryHelper.CreateSmallRectangle(startVertex, tolerance);
                    // Get all the lines in current processing shape bounds.
                    Collection<Feature> adjacentFeatures = featureSource.GetFeaturesInsideBoundingBox(startSmallBounds, ReturningColumnsType.NoColumns);
                    // Loop and see if the queried shape is intersected with processing shape.
                    foreach (Feature adjacentFeature in adjacentFeatures)
                    {
                        // Given the adjacent line is line shape, and create adjacent list without un-direction consideration.
                        LineShape adjacentLineShape = GeometryHelper.GetLineShapes(feature)[0];

                        // Check if it's start vertext or end vertex of the adjacent line shape.
                        int vertexIndex = 0;
                        if (GeometryHelper.IsSamePoint(startVertex, adjacentLineShape.Vertices[adjacentLineShape.Vertices.Count - 1], tolerance))
                        {
                            vertexIndex = adjacentLineShape.Vertices.Count - 1;
                        }
                    }


                    Vertex endVertex = processingLineShape.Vertices[processingLineShape.Vertices.Count - 1];

                }
            }
            featureSource.Close();


            return roadNetwork;
        }

        public virtual void ImportData(FeatureSource featureSourceForRead, FeatureSource featureSourceForSave)
        {
            featureSourceForRead.Open();
            featureSourceForSave.Open();

            long featureCount = featureSourceForRead.GetCount();
            Collection<Feature> features = featureSourceForRead.GetAllFeatures(ReturningColumnsType.NoColumns);
            foreach (Feature feature in features)
            {
                Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
                // Get the lineshape of the processing feature.
                foreach (LineShape processingLineShape in processingLineShapes)
                {
                    // Define a variable to save the points where the adjacent lines intersect with current processing line.
                    Collection<PointShape> crossingPoints = new Collection<PointShape>();
                    
                    // Get all the lines in current processing shape bounds.
                    Collection<Feature> adjacentFeatures = featureSourceForRead.GetFeaturesInsideBoundingBox(processingLineShape.GetBoundingBox(), ReturningColumnsType.NoColumns);
                    
                    // Loop and see if the queried shape is intersected with processing shape.
                    foreach (Feature adjacentFeature in adjacentFeatures)
                    {
                        LineBaseShape adjacentLineShape = adjacentFeature.GetShape() as LineBaseShape;
                        MultipointShape tempCrossingPoints = processingLineShape.GetCrossing(adjacentLineShape);

                        // The queried shape is intersected with processing shape.
                        foreach (PointShape point in tempCrossingPoints.Points)
                        {
                            crossingPoints.Add(point);
                        }
                    }

                    // Order the crossing points following the sequence of line vertex.
                    Dictionary<Vertex, bool> vertecesOfNewLine = GeometryHelper.AddCrossingPointToLine(processingLineShape, crossingPoints);

                    // Split current processing lineshape into segments.
                    featureSourceForSave.BeginTransaction();
                    Collection<Vertex> verteces = new Collection<Vertex>();
                    foreach (var vertex in vertecesOfNewLine)
                    {
                        verteces.Add(vertex.Key);
                        if (vertex.Value)
                        {
                            if (verteces.Count >= 2)
                            {
                                LineShape segment = new LineShape(verteces);
                                featureSourceForSave.AddFeature(new Feature(segment));

                                verteces.Clear();
                            }
                        }
                    }
                    featureSourceForSave.CommitTransaction();
                }

                Console.WriteLine(string.Format("Done {0} in {1}", feature.Id, featureCount));
            }

            featureSourceForRead.Close();
            featureSourceForSave.Close();
        }
    }
}
