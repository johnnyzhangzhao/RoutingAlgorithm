using RouteAlgorithm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    public class StreamSource
    {
        internal const int VERSION = 3;

        private double tolerance;
        private string onewayColumn;

        public StreamSource()
        {
            tolerance = 1e-6;
            DataUnit = GeographyUnit.DecimalDegree;
            DistanceUnit = DistanceUnit.Meter;
        }

        public double Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }

        public GeographyUnit DataUnit
        {
            get;
            set;
        }

        public DistanceUnit DistanceUnit
        {
            get;
            set;
        }

        public string OnewayColumn
        {
            get { return onewayColumn; }
            set { onewayColumn = value; }
        }

        public virtual void CreateIndex(RoadNetwork roadNetwork)
        {

        }

        public virtual RoadNetwork CreateNetwork(FeatureSource featureSource)
        {
            RoadNetwork roadNetwork = new RoadNetwork();

            featureSource.Open();
            Collection<Feature> features = featureSource.GetAllFeatures(ReturningColumnsType.NoColumns);

            //Stack<Feature> featureStack = new Stack<Feature>(features);
            //while (featureStack.Count > 0)
            //{
            //    Feature feature = featureStack.Pop();
            //    if (features.Contains(feature))
            //    {
            //        features.Remove(feature);
            //    }
            //    else
            //    {
            //        continue;
            //    }

            //    LineShape shape = feature.GetShape() as LineShape;
            //    Node startNode = new Node(feature.Id, (float)shape.Vertices[0].Y, (float)shape.Vertices[0].X);
            //    var featureBoundingBox = feature.GetBoundingBox();
            //    Collection<Feature> containsFeatures = featureSource.GetFeaturesInsideBoundingBox(featureBoundingBox, ReturningColumnsType.AllColumns);

            //    foreach (var item in containsFeatures)
            //    {
            //        if (item.Crosses(feature))
            //        {
            //            features.Remove(item);
            //            startNode.IncomingArcs.Add(new Arc())
            //        }
            //    }
            //}
            foreach (Feature feature in features)
            {
                Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
                // Get the lineshape of the processing feature.
                foreach (LineShape processingLineShape in processingLineShapes)
                {
                    Node startNode = CreateNode(featureSource, roadNetwork, processingLineShape.Vertices[0]);
                    if (!roadNetwork.Nodes.Any(node => node.Id == startNode.Id))
                    {
                        roadNetwork.Nodes.Add(startNode);
                    }

                    Node endNode = CreateNode(featureSource, roadNetwork, processingLineShape.Vertices[processingLineShape.Vertices.Count - 1]);
                    if (!roadNetwork.Nodes.Any(node => node.Id == endNode.Id))
                    {
                        roadNetwork.Nodes.Add(endNode);
                    }
                }
            }
            featureSource.Close();

            return roadNetwork;
        }

        public virtual bool IsRoadDirectionAccessable(Feature feature, RoadDirection roadDirection)
        {
            // Todo: check one-way roads is right to the specific direction.
            bool isRoadDirectionAccessable = false;
            string onewayValue = feature.ColumnValues["oneway"];
            if(String.Compare(onewayValue.Trim(), "1", true, CultureInfo.InvariantCulture) == 0)
            {
                isRoadDirectionAccessable = true;
            }
            else
            {
                isRoadDirectionAccessable = false;
            }

            return isRoadDirectionAccessable;
        }

        public virtual float CalculateRoadCost(LineShape lineShape)
        {
            return (float)lineShape.GetLength(DataUnit, DistanceUnit);
        }

        public virtual void ImportData(FeatureSource featureSourceForRead, FeatureSource featureSourceForSave)
        {
            featureSourceForRead.Open();
            featureSourceForSave.Open();

            long featureCount = featureSourceForRead.GetCount();
            Collection<Feature> features = featureSourceForRead.GetAllFeatures(ReturningColumnsType.AllColumns);
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
                            bool hasAdded = false;
                            foreach (var item in crossingPoints)
                            {
                                if (point.X == item.X && point.Y == item.Y)
                                {
                                    hasAdded = true;
                                    break;
                                }
                            }
                            if (!hasAdded)
                            {
                                crossingPoints.Add(point);
                            }
                        }
                    }

                    // Order the crossing points following the sequence of line vertex.
                    Collection<FlagedVertex> vertecesOfNewLine = GeometryHelper.AddCrossingPointToLine(processingLineShape, crossingPoints);

                    // Split current processing lineshape into segments.
                    featureSourceForSave.BeginTransaction();
                    Collection<Vertex> verteces = new Collection<Vertex>();
                    foreach (var vertex in vertecesOfNewLine)
                    {
                        verteces.Add(vertex.Vertex);
                        if (vertex.Flag)
                        {
                            if (verteces.Count >= 2)
                            {
                                LineShape segment = new LineShape(verteces);
                                featureSourceForSave.AddFeature(new Feature(segment, feature.ColumnValues));

                                verteces.RemoveAt(0);
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

        private Node CreateNode(FeatureSource featureSource, RoadNetwork roadNetwork, Vertex vertex)
        {
            // Create node based on vertex.
            // Todo: check if it has been existed.
            Collection<Feature> adjacentTailNodeFeatures = GetAdjacentFeaturesOfVertex(featureSource, vertex);
            Node tailNode = InitializeNodeFromVeterx(vertex, adjacentTailNodeFeatures);
            roadNetwork.Nodes.Add(tailNode);

            // Loop and see if the queried shape is intersected with processing shape.
            foreach (Feature adjacentTailNodeFeature in adjacentTailNodeFeatures)
            {
                // Given the adjacent line is line shape, and create adjacent list without un-direction consideration.
                LineShape adjacentLineShape = GeometryHelper.GetLineShapes(adjacentTailNodeFeature)[0];
                adjacentLineShape.Id = adjacentTailNodeFeature.Id;


                // Check if it's start vertext or end vertex of the adjacent line shape.
                int vertexIndex = 0;
                if (GeometryHelper.IsSamePoint(vertex, adjacentLineShape.Vertices[adjacentLineShape.Vertices.Count - 1], tolerance))
                {
                    vertexIndex = adjacentLineShape.Vertices.Count - 1;
                }

                // Todo: check if it has been existed.
                Collection<Feature> adjacentHeadNodeFeatures = GetAdjacentFeaturesOfVertex(featureSource, adjacentLineShape.Vertices[vertexIndex]);
                Node headNode = InitializeNodeFromVeterx(adjacentLineShape.Vertices[vertexIndex], adjacentHeadNodeFeatures);
                roadNetwork.Nodes.Add(headNode);

                Arc adjacentArc = new Arc(adjacentTailNodeFeature.Id, headNode, tailNode, CalculateRoadCost(adjacentLineShape));

                // Check if the direction is the allowed of current segment?
                RoadDirection roadDirection = CalculateRoadDirection(adjacentTailNodeFeature, adjacentLineShape, vertex);
                if (roadDirection == RoadDirection.Forward)
                {
                    tailNode.OutgoingArcs.Add(adjacentArc);
                }
                else if (roadDirection == RoadDirection.Backward)
                {
                    tailNode.IncomingArcs.Add(adjacentArc);
                }
            }

            return tailNode;
        }

        private RoadDirection CalculateRoadDirection(Feature feature, LineShape lineShape, Vertex startVertex)
        {
            int index = 0;  // Given the vertex is the first of input line shape.
            if (!GeometryHelper.IsSamePoint(lineShape.Vertices[0], startVertex, Tolerance))
            {
                index = lineShape.Vertices.Count - 1;
            }

            RoadDirection direction = RoadDirection.Noway;
            if (index == 0)
            {
                if (IsRoadDirectionAccessable(feature, RoadDirection.Forward))
                {
                    direction = RoadDirection.Forward;
                }
            }
            else
            {
                if (IsRoadDirectionAccessable(feature, RoadDirection.Backward))
                {
                    direction = RoadDirection.Backward;
                }
            }

            return direction;
        }

        private Collection<Feature> GetAdjacentFeaturesOfVertex(FeatureSource featureSource, Vertex vertex)
        {
            RectangleShape startSmallBounds = GeometryHelper.CreateSmallRectangle(vertex, tolerance);
            // Get all the lines in current processing shape bounds.
            Collection<Feature> adjacentFeatures = featureSource.GetFeaturesInsideBoundingBox(startSmallBounds, ReturningColumnsType.AllColumns);

            return adjacentFeatures;
        }

        private Node InitializeNodeFromVeterx(Vertex vertex, Collection<Feature> adjacentFeatures)
        {
            List<string> ids = new List<string>();
            foreach (var feature in adjacentFeatures)
            {
                ids.Add(feature.Id);
            }
            ids.Sort((x, y) => string.Compare(x, y));
            string id = string.Join("_", ids.ToArray());

            Node node = new Node(id, (float)vertex.Y, (float)vertex.X);
            return node;
        }
    }
}
