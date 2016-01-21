using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    public class StreamSource
    {
        internal const int VERSION = 3;

        private double tolerance;
        private string onewayColumn;

        private Queue<Collection<Feature>> queue;

        public StreamSource()
        {
            tolerance = 1e-6;
            DataUnit = GeographyUnit.DecimalDegree;
            DistanceUnit = DistanceUnit.Meter;
            queue = new Queue<Collection<Feature>>();
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
            featureSource.Open();
            QTreeSpatialIndex qtree = new QTreeSpatialIndex(featureSource.GetBoundingBox());

#if DEBUG
            long featureCount = featureSource.GetCount();
#endif

            RoadNetwork roadNetwork = new RoadNetwork();
            Collection<Feature> features = featureSource.GetAllFeatures(ReturningColumnsType.NoColumns);
            featureSource.Close();
            Collection<Collection<Feature>> featureGroups = GroupFeatures(features);
            int done = 0;
            var tasks = (from items in featureGroups
                         select Task.Factory.StartNew(() =>
                         {
                             var clonedFeatureSource = featureSource.CloneDeep();
                             clonedFeatureSource.Open();
                             foreach (var feature in items)
                             {
                                 Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
                                 // Get the lineshape of the processing feature.
                                 foreach (LineShape processingLineShape in processingLineShapes)
                                 {
                                     BuildNetworkNode(clonedFeatureSource, qtree, roadNetwork, processingLineShape.Vertices[0]);
                                     BuildNetworkNode(clonedFeatureSource, qtree, roadNetwork, processingLineShape.Vertices[processingLineShape.Vertices.Count - 1]);
                                 }
                                 done++;
                                 Console.WriteLine(string.Format("Done {0} in {1}", feature.Id, done));
                             }

                         })).ToArray();
            //foreach (Feature feature in features)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
            //        // Get the lineshape of the processing feature.
            //        foreach (LineShape processingLineShape in processingLineShapes)
            //        {
            //            BuildNetworkNode(featureSource, qtree, roadNetwork, processingLineShape.Vertices[0]);
            //            BuildNetworkNode(featureSource, qtree, roadNetwork, processingLineShape.Vertices[processingLineShape.Vertices.Count - 1]);
            //        }
            //    }
            //   );
            Task.WaitAll(tasks);

#if DEBUG

#endif

            //}
            //featureSource.Close();

            return roadNetwork;
        }

        private static Collection<Collection<Feature>> GroupFeatures(Collection<Feature> features)
        {
            Collection<Collection<Feature>> featureGroups = new Collection<Collection<Feature>>();
            const int threadCount = 8;
            for (int i = 0; i < threadCount; i++)
            {
                featureGroups.Add(new Collection<Feature>());
            }

            for (int i = 0; i < features.Count; i++)
            {
                featureGroups[i % threadCount].Add(features[i]);
            }

            return featureGroups;
        }

        public virtual bool IsRoadDirectionAccessable(Feature feature, RoadDirection roadDirection)
        {
            // Todo: check one-way roads is right to the specific direction.
            bool isRoadDirectionAccessable = false;
            string onewayValue = feature.ColumnValues["oneway"];
            if (String.Compare(onewayValue.Trim(), "1", true, CultureInfo.InvariantCulture) == 0)
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

        private void WriteFeaturesIntoQueue(Collection<Feature> features, FeatureSource featureSource)
        {
            foreach (Feature feature in features)
            {
                Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
                // Get the lineshape of the processing feature.
                foreach (LineShape processingLineShape in processingLineShapes)
                {
                    // Define a variable to save the points where the adjacent lines intersect with current processing line.
                    Collection<PointShape> crossingPoints = new Collection<PointShape>();

                    // Get all the lines in current processing shape bounds.
                    Collection<Feature> adjacentFeatures = featureSource.GetFeaturesInsideBoundingBox(processingLineShape.GetBoundingBox(), ReturningColumnsType.NoColumns);

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
                    Collection<Vertex> verteces = new Collection<Vertex>();
                    Collection<Feature> lineFeatures = new Collection<Feature>();
                    foreach (var vertex in vertecesOfNewLine)
                    {
                        verteces.Add(vertex.Vertex);
                        if (vertex.Flag)
                        {
                            if (verteces.Count >= 2)
                            {
                                LineShape segment = new LineShape(verteces);
                                lineFeatures.Add(new Feature(segment, feature.ColumnValues));
                                verteces.RemoveAt(0);
                            }
                        }
                    }
                    if (lineFeatures.Count > 0)
                    {
                        queue.Enqueue(lineFeatures);
                    }
                }

#if DEBUG
                Console.WriteLine(string.Format("Done {0} in {1}", feature.Id, features.Count));
#endif
            }
        }

        private void SaveFeatures(Collection<Feature> features, FeatureSource featureSource)
        {
            featureSource.BeginTransaction();
            foreach (var feature in features)
            {
                featureSource.AddFeature(feature);
            }
            featureSource.CommitTransaction();
        }

        public virtual void ImportData(FeatureSource featureSourceForRead, FeatureSource featureSourceForSave)
        {
            bool readCompleted = false;
            featureSourceForRead.Open();
            featureSourceForSave.Open();
#if DEBUG
            long featureCount = featureSourceForRead.GetCount();
#endif
            Collection<Feature> features = featureSourceForRead.GetAllFeatures(ReturningColumnsType.AllColumns);
            Collection<Collection<Feature>> featureGroups = GroupFeatures(features);
            var writeTasks = (from items in featureGroups
                              select Task.Factory.StartNew(() => WriteFeaturesIntoQueue(items, featureSourceForRead))).ToArray();


            var readTask = Task.Factory.StartNew(() =>
            {
                while (!readCompleted || queue.Count > 0)
                {
                    while (queue.Count > 0)
                    {
                        Collection<Feature> lineFeatures = queue.Dequeue();
                        SaveFeatures(lineFeatures, featureSourceForSave);
                    }
                }
            });

            Task.WaitAll(writeTasks);
            readCompleted = true;
            Task.WaitAny(readTask);
            #region Old version
            //foreach (Feature feature in features)
            //{
            //    Collection<LineShape> processingLineShapes = GeometryHelper.GetLineShapes(feature);
            //    // Get the lineshape of the processing feature.
            //    foreach (LineShape processingLineShape in processingLineShapes)
            //    {
            //        // Define a variable to save the points where the adjacent lines intersect with current processing line.
            //        Collection<PointShape> crossingPoints = new Collection<PointShape>();

            //        // Get all the lines in current processing shape bounds.
            //        Collection<Feature> adjacentFeatures = featureSourceForRead.GetFeaturesInsideBoundingBox(processingLineShape.GetBoundingBox(), ReturningColumnsType.NoColumns);

            //        // Loop and see if the queried shape is intersected with processing shape.
            //        foreach (Feature adjacentFeature in adjacentFeatures)
            //        {
            //            LineBaseShape adjacentLineShape = adjacentFeature.GetShape() as LineBaseShape;
            //            MultipointShape tempCrossingPoints = processingLineShape.GetCrossing(adjacentLineShape);

            //            // The queried shape is intersected with processing shape.
            //            foreach (PointShape point in tempCrossingPoints.Points)
            //            {
            //                bool hasAdded = false;
            //                foreach (var item in crossingPoints)
            //                {
            //                    if (point.X == item.X && point.Y == item.Y)
            //                    {
            //                        hasAdded = true;
            //                        break;
            //                    }
            //                }
            //                if (!hasAdded)
            //                {
            //                    crossingPoints.Add(point);
            //                }
            //            }
            //        }

            //        // Order the crossing points following the sequence of line vertex.
            //        Collection<FlagedVertex> vertecesOfNewLine = GeometryHelper.AddCrossingPointToLine(processingLineShape, crossingPoints);

            //        // Split current processing lineshape into segments.
            //        featureSourceForSave.BeginTransaction();
            //        Collection<Vertex> verteces = new Collection<Vertex>();
            //        foreach (var vertex in vertecesOfNewLine)
            //        {
            //            verteces.Add(vertex.Vertex);
            //            if (vertex.Flag)
            //            {
            //                if (verteces.Count >= 2)
            //                {
            //                    LineShape segment = new LineShape(verteces);
            //                    featureSourceForSave.AddFeature(new Feature(segment, feature.ColumnValues));

            //                    verteces.RemoveAt(0);
            //                }
            //            }
            //        }
            //        featureSourceForSave.CommitTransaction();
            //    }

            //#if DEBUG
            //Console.WriteLine(string.Format("Done {0} in {1}", feature.Id, featureCount));
            //#endif
            //}

            #endregion
            featureSourceForRead.Close();
            featureSourceForSave.Close();
        }

        private void BuildNetworkNode(FeatureSource featureSource, QTreeSpatialIndex qtree, RoadNetwork roadNetwork, Vertex vertex)
        {
            RectangleShape startSmallBounds = GeometryHelper.CreateSmallRectangle(vertex, tolerance);


            Collection<string> idsInside = qtree.GetFeatureIdsIntersectingBoundingBox(startSmallBounds);

            if (idsInside.Count <= 0)
            {
                Node startNode = CreateNode(featureSource, roadNetwork, vertex);
                roadNetwork.Nodes.Add(startNode);
                qtree.Add(new PointShape(vertex));
            }
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
            //List<string> ids = new List<string>();
            //foreach (var feature in adjacentFeatures)
            //{
            //    ids.Add(feature.Id);
            //}
            //ids.Sort((x, y) => string.Compare(x, y));
            //string id = adjacentFeatures[0].Id; //string.Join("_", ids.ToArray());

            Node node = new Node(string.Empty, (float)vertex.Y, (float)vertex.X);
            return node;
        }
    }
}
