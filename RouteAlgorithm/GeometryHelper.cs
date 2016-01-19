using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    internal class GeometryHelper
    {
        internal static Collection<FlagedVertex> AddCrossingPointToLine(LineShape line, Collection<PointShape> crossingPoints)
        {
            Collection<FlagedVertex> orderedPoints = new Collection<FlagedVertex>();
            for (int i = 0; i < line.Vertices.Count - 1; i++)
            {
                // Add the first vertex of segment.
                if (!HasExists(orderedPoints, line.Vertices[i]))
                {
                    orderedPoints.Add(new FlagedVertex() { Vertex = line.Vertices[i] , Flag = true});
                }

                Dictionary<PointShape, double> pointsOnSegement = new Dictionary<PointShape, double>();
                foreach (PointShape crossingPoint in crossingPoints)
                {
                    // Add the crossing point if it's on the segment.
                    bool isIntermiate = IsIntermediatePoint(line.Vertices[i], line.Vertices[i + 1], crossingPoint);
                    if (isIntermiate)
                    {
                        double pointDistance = GetEvaluatedDistance(line.Vertices[i], crossingPoint);
                        pointsOnSegement.Add(crossingPoint, pointDistance);
                    }
                }

                if (pointsOnSegement.Count > 0)
                {
                    pointsOnSegement.OrderBy(v => v.Value);
                    foreach (var pointOnSegment in pointsOnSegement)
                    {
                        Vertex vertex = new Vertex(pointOnSegment.Key.X, pointOnSegment.Key.Y);
                        if (!HasExists(orderedPoints, vertex))
                        {
                            bool isOnTheLine = false;
                            foreach (var item in line.Vertices)
                            {
                                if(vertex.X == item.X && vertex.Y == item.Y)
                                {
                                    isOnTheLine = true;
                                    break;
                                }
                            }

                            if(isOnTheLine)
                            {
                                orderedPoints.Add(new FlagedVertex() { Vertex = vertex, Flag = true });
                            }
                            else
                            { 
                                orderedPoints.Add(new FlagedVertex() { Vertex = vertex, Flag = false });
                            }
                        }
                        crossingPoints.Remove(pointOnSegment.Key);
                    }
                }
            }

            if(line.Vertices.Count > orderedPoints.Count)
            {
                bool isLoop = false;
                foreach (var item in orderedPoints)
                {
                    if (line.Vertices[line.Vertices.Count - 1].X == item.Vertex.X && line.Vertices[line.Vertices.Count - 1].Y == item.Vertex.Y)
                    {
                        isLoop = true;
                        break;
                    }
                }

                if(isLoop)
                {
                    orderedPoints.Add(new FlagedVertex() { Vertex = new Vertex(line.Vertices[line.Vertices.Count - 1].X, line.Vertices[line.Vertices.Count - 1].Y), Flag = true });
                }
            }

            return orderedPoints;
        }

        private static bool HasExists(Collection<FlagedVertex> flagVertices, Vertex vertex )
        {
            bool hasExists = false;

            foreach (var item in flagVertices)
            {
                if(item.Vertex.X == vertex.X && item.Vertex.Y == vertex.Y)
                {
                    hasExists = true;
                    break;
                }
            }

            return hasExists;
        }

        internal static bool IsIntermediatePoint(Vertex startVertex, Vertex endVertex, PointShape intermeidatePoint)
        {
            bool isIntermediatePoint = false;

            if ((startVertex.X == intermeidatePoint.X && startVertex.Y == intermeidatePoint.Y)
                || (endVertex.X == intermeidatePoint.X && endVertex.Y == intermeidatePoint.Y))
            {
                isIntermediatePoint = true;
            }
            else
            {
                if (endVertex.X == startVertex.X)
                {
                    if (intermeidatePoint.X == startVertex.X)
                    {
                        if ((intermeidatePoint.Y >= startVertex.Y && intermeidatePoint.Y <= endVertex.Y)
                            || (intermeidatePoint.Y >= endVertex.Y && intermeidatePoint.Y <= startVertex.Y))
                        {
                            isIntermediatePoint = true;
                        }
                    }
                }
                else
                {
                    double segmentRatio = (endVertex.Y - startVertex.Y) / (endVertex.X - startVertex.X);
                    double intermeidateRatio = (intermeidatePoint.Y - startVertex.Y) / (intermeidatePoint.X - startVertex.X);

                    if (segmentRatio == intermeidateRatio)
                    {
                        if ((intermeidatePoint.X >= startVertex.X && intermeidatePoint.X <= endVertex.X)
                            || (intermeidatePoint.X >= endVertex.X && intermeidatePoint.Y <= startVertex.X))
                        {
                            isIntermediatePoint = true;
                        }
                    }
                }
            }
            return isIntermediatePoint;
        }

        internal static double GetEvaluatedDistance(Vertex point1, PointShape point2)
        {
            return DecimalDegreesHelper.GetDistanceFromDecimalDegrees(new PointShape(point1), point2, DistanceUnit.Meter);
        }

        internal static bool IsCrossingPoint(Vertex vertex, Collection<PointShape> points, double tolerance)
        {
            foreach (PointShape point in points)
            {
                if (IsSamePoint(point, vertex, tolerance))
                {
                    return true;
                }
            }

            return false;
        }

        internal static RectangleShape CreateSmallRectangle(Vertex vertex, double tolerance)
        {
            RectangleShape bbox = new RectangleShape();
            double miniTolerance = tolerance / 2;
            bbox.UpperLeftPoint.X = vertex.X - miniTolerance;
            bbox.UpperLeftPoint.Y = vertex.Y + miniTolerance;
            bbox.LowerRightPoint.X = vertex.X + miniTolerance;
            bbox.LowerRightPoint.Y = vertex.Y - miniTolerance;

            return bbox;
        }

        internal static bool IsSamePoint(Vertex point1, Vertex point2, double tolerance)
        {
            return isSamePoint(point1.X, point1.Y, point2.X, point2.Y, tolerance);
        }

        internal static bool IsSamePoint(PointShape point1, Vertex point2, double tolerance)
        {
            return isSamePoint(point1.X, point1.Y, point2.X, point2.Y, tolerance);
        }

        private static bool isSamePoint(double point1X, double point1Y, double point2X, double point2Y, double tolerance)
        {
            bool isEqual = false;
            if (Math.Abs(point1X - point2X) < tolerance && Math.Abs(point1Y - point2Y) < tolerance)
            {
                isEqual = true;
            }

            return isEqual;
        }

        internal static Collection<LineShape> GetLineShapes(Feature feature)
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
