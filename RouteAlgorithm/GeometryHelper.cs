using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    internal class GeometryHelper
    {
        internal static Dictionary<Vertex, bool> AddCrossingPointToLine(LineShape line, Collection<PointShape> crossingPoints)
        {
            Dictionary<Vertex, bool> orderedPoints = new Dictionary<Vertex, bool>();
            for (int i = 0; i < line.Vertices.Count - 1; i++)
            {
                Dictionary<double, PointShape> pointsOnSegement = new Dictionary<double, PointShape>();
                foreach (PointShape crossingPoint in crossingPoints)
                {
                    // Add the first vertex of segment.
                    orderedPoints.Add(line.Vertices[i], true);

                    // Add the crossing point if it's on the segment.
                    bool isIntermiate = IsIntermediatePoint(line.Vertices[i], line.Vertices[i + 1], crossingPoint);
                    if (isIntermiate)
                    {
                        double pointDistance = GetEvaluatedDistance(line.Vertices[i], crossingPoint);
                        pointsOnSegement.Add(pointDistance, crossingPoint);
                    }
                }

                if (pointsOnSegement.Count > 0)
                {
                    pointsOnSegement.OrderBy(v => v.Key);
                    foreach (var pointOnSegment in pointsOnSegement)
                    {
                        orderedPoints.Add(new Vertex(pointOnSegment.Value.X, pointOnSegment.Value.Y), false);
                        crossingPoints.Remove(pointOnSegment.Value);
                    }
                }
            }
            orderedPoints.Add(line.Vertices[line.Vertices.Count - 1], true);

            return orderedPoints;
        }

        internal static bool IsIntermediatePoint(Vertex startVetex, Vertex endVertex, PointShape intermeidatePoint)
        {
            return true;
        }

        internal static double GetEvaluatedDistance(Vertex point1, PointShape point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
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
