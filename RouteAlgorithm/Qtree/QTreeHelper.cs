using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    [Serializable]
    internal static class QTreeHelper
    {
        public static QuadCell GetCellByLocation(RectangleShape extent, string location)
        {
            RectangleShape boundingBox = SquaredExtent(extent);
            for (int i = 0; i < location.Length; i++)
            {
                switch (location[i].ToString())
                {
                    case "0":
                        if (i != 0)
                        {
                            throw new Exception("parameter is invalid.");
                        }
                        else
                        {
                            break;
                        }
                    case "1":
                        boundingBox = GetUpperLeftQuater(boundingBox);
                        break;
                    case "2":
                        boundingBox = GetUpperRightQuater(boundingBox);
                        break;
                    case "3":
                        boundingBox = GetLowerLeftQuater(boundingBox);
                        break;
                    case "4":
                        boundingBox = GetLowerRightQuater(boundingBox);
                        break;
                    default:
                        throw new Exception("parameter is invalid.");
                }
            }
            return new QuadCell(location, boundingBox);
        }

        public static string GetLocation(RectangleShape extent, Feature feature, int level)
        {
            return GetLocation(extent, feature.GetShape(), level);
        }

        public static string GetLocation(RectangleShape extent, BaseShape shape, int level)
        {
            RectangleShape extentBoundingBox = SquaredExtent(extent);
            RectangleShape shapeBoundingBox = shape.GetBoundingBox();

            string location = string.Empty;

            if (IsWithin(shapeBoundingBox, extentBoundingBox))
            {
                location += "0";
                int currentlevel = 1;

                while (currentlevel < level)
                {
                    RectangleShape upperLeft = GetUpperLeftQuater(extentBoundingBox);
                    RectangleShape uppperRight = GetUpperRightQuater(extentBoundingBox);
                    RectangleShape lowerLeft = GetLowerLeftQuater(extentBoundingBox);
                    RectangleShape lowerRight = GetLowerRightQuater(extentBoundingBox);

                    if (IsWithin(shapeBoundingBox,upperLeft))
                    {
                        location += "1";
                        currentlevel++;
                        extentBoundingBox = upperLeft;
                    }
                    else if (IsWithin(shapeBoundingBox, uppperRight))
                    {
                        location += "2";
                        currentlevel++;
                        extentBoundingBox = uppperRight;
                    }
                    else if (IsWithin(shapeBoundingBox, lowerLeft))
                    {
                        location += "3";
                        currentlevel++;
                        extentBoundingBox = lowerLeft;
                    }
                    else if (IsWithin(shapeBoundingBox, lowerRight))
                    {
                        location += "4";
                        currentlevel++;
                        extentBoundingBox = lowerRight;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return location;

        }

        private static bool IsWithin(RectangleShape rectangle1, RectangleShape rectangle2)
        {
            bool isWithin = false;

            if(!(rectangle1.UpperRightPoint.X < rectangle2.UpperLeftPoint.X || rectangle1.UpperLeftPoint.X > rectangle2.UpperRightPoint.X
                || rectangle1.UpperLeftPoint.Y < rectangle2.LowerLeftPoint.Y || rectangle1.LowerLeftPoint.Y > rectangle2.UpperLeftPoint.Y))
            {
                isWithin = true;
            }

            return isWithin;
        }

        public static int GetAppropriateLevel(RectangleShape extent, Feature feature)
        {
            return GetAppropriateLevel(extent, feature.GetShape());
        }

        public static int GetAppropriateLevel(RectangleShape extent, BaseShape shape)
        {
            RectangleShape currentBoundingBox = SquaredExtent(extent);
            RectangleShape shapeBoundingBox = shape.GetBoundingBox();

            int level = 0;

            if (shapeBoundingBox.IsWithin(currentBoundingBox))
            {
                while (true)
                {
                    level++;
                    RectangleShape upperLeft = GetUpperLeftQuater(currentBoundingBox);
                    RectangleShape uppperRight = GetUpperRightQuater(currentBoundingBox);
                    RectangleShape lowerLeft = GetLowerLeftQuater(currentBoundingBox);
                    RectangleShape lowerRight = GetLowerRightQuater(currentBoundingBox);

                    if (shapeBoundingBox.IsWithin(upperLeft))
                    {
                        currentBoundingBox = upperLeft;
                    }
                    else if (shapeBoundingBox.IsWithin(uppperRight))
                    {
                        currentBoundingBox = uppperRight;
                    }
                    else if (shapeBoundingBox.IsWithin(lowerLeft))
                    {
                        currentBoundingBox = lowerLeft;
                    }
                    else if (shapeBoundingBox.IsWithin(lowerRight))
                    {
                        currentBoundingBox = lowerRight;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return level;
        }

        public static Collection<QuadCell> GetIntersectingCells(RectangleShape extent, Feature feature, int startLevel, int endLevel)
        {
            return GetIntersectingCells(extent, feature.GetShape(), startLevel, endLevel);
        }

        public static Collection<QuadCell> GetIntersectingCells(RectangleShape extent, BaseShape shape, int startLevel, int endLevel)
        {
            RectangleShape extentBoundingBox = SquaredExtent(extent);
            RectangleShape shapeBoundingBox = shape.GetBoundingBox();

            Collection<QuadCell> result = new Collection<QuadCell>();

            if (shapeBoundingBox.Intersects(extentBoundingBox))
            {
                string location = GetLocation(extentBoundingBox, shape, int.MaxValue);

                if (startLevel <= location.Length)
                {
                    for (int x = startLevel; x <= endLevel; x++)
                    {
                        if (x <= location.Length)
                        {
                            if (location.Length > 1)
                            {
                                result.Add(GetCellByLocation(extentBoundingBox, "0" + location.Substring(1, x - 1)));
                            }
                            else
                            {
                                result.Add(GetCellByLocation(extentBoundingBox, "0"));
                            }
                        }
                    }
                }
                if (endLevel > location.Length)
                {
                    Queue<string> locationsToProcess = new Queue<string>();
                    locationsToProcess.Enqueue(location);

                    while (locationsToProcess.Count > 0)
                    {
                        string currentLocation = locationsToProcess.Dequeue();

                        for (int k = 1; k <= 4; k++)
                        {
                            string newLocation = currentLocation + k;
                            result.Add(GetCellByLocation(extentBoundingBox, newLocation));
                            if (newLocation.Length < endLevel)
                            {
                                locationsToProcess.Enqueue(newLocation);
                            }
                        }
                    }


                }
            }
            //if (result.Count == 0)
            //{
            //    result.Add(GetCellByLocation(extentBoundingBox, "01"));
            //    result.Add(GetCellByLocation(extentBoundingBox, "02"));
            //    result.Add(GetCellByLocation(extentBoundingBox, "03"));
            //    result.Add(GetCellByLocation(extentBoundingBox, "04"));
            //}
            return result;
        }

        public static int GetLevelByCellWidth(RectangleShape extent, double cellWidth)
        {
            return GetLevelByCellWidth(extent, GeographyUnit.Meter, cellWidth, DistanceUnit.Meter);
        }

        public static int GetLevelByCellWidth(RectangleShape extent, GeographyUnit extentMapUnit, double cellWidth, DistanceUnit widthDistanceUnit)
        {
            PointShape centerPoint = extent.GetCenterPoint();

            PointShape centerLeft = new PointShape(extent.LowerLeftPoint.X, centerPoint.Y);
            PointShape centerRight = new PointShape(extent.UpperRightPoint.X, centerPoint.Y);
            PointShape centerUpper = new PointShape(centerPoint.X, extent.UpperLeftPoint.Y);
            PointShape centerLower = new PointShape(centerPoint.X, extent.LowerLeftPoint.Y);

            double centerWidth = centerLeft.GetDistanceTo(centerRight, extentMapUnit, widthDistanceUnit);
            double centerHeight = centerUpper.GetDistanceTo(centerLower, extentMapUnit, widthDistanceUnit);

            double longestEdge = Math.Max(centerWidth, centerHeight);

            double level = Math.Round(2 * Math.Log((longestEdge / cellWidth), 4) + 1);

            return (int)level;
        }

        public static double GetCellWidthByLevel(RectangleShape extent, int level)
        {
            return GetCellWidthByLevel(extent, GeographyUnit.Meter, level, DistanceUnit.Meter);
        }

        public static double GetCellWidthByLevel(RectangleShape extent, GeographyUnit extentMapUnit, int level, DistanceUnit returnDistanceUnit)
        {
            PointShape centerPoint = extent.GetCenterPoint();

            PointShape centerLeft = new PointShape(extent.LowerLeftPoint.X, centerPoint.Y);
            PointShape centerRight = new PointShape(extent.UpperRightPoint.X, centerPoint.Y);
            PointShape centerUpper = new PointShape(centerPoint.X, extent.UpperLeftPoint.Y);
            PointShape centerLower = new PointShape(centerPoint.X, extent.LowerLeftPoint.Y);

            double centerWidth = centerLeft.GetDistanceTo(centerRight, extentMapUnit, returnDistanceUnit);
            double centerHeight = centerUpper.GetDistanceTo(centerLower, extentMapUnit, returnDistanceUnit);
            double longestEdge = Math.Max(centerWidth, centerHeight);

            double cellWidth = longestEdge / Math.Sqrt(Math.Pow(4, level - 1));
            return cellWidth;
        }

        private static RectangleShape SquaredExtent(RectangleShape extent)
        {
            PointShape center = extent.GetCenterPoint();

            double extentWidth = extent.LowerRightPoint.X - extent.LowerLeftPoint.X;
            double extentHeight = extent.UpperLeftPoint.Y - extent.LowerLeftPoint.Y;

            double halfEdgeLength = (extentWidth > extentHeight) ? extentWidth / 2 : extentHeight / 2;

            PointShape upperLeft = new PointShape(center.X - halfEdgeLength, center.Y + halfEdgeLength);
            PointShape lowerRight = new PointShape(center.X + halfEdgeLength, center.Y - halfEdgeLength);

            return new RectangleShape(upperLeft, lowerRight);
        }

        private static Dictionary<int, Collection<QuadCell>> GetCells(string location, RectangleShape boudingBox, Dictionary<int, Collection<QuadCell>> currentLevelCellsSet, int currentLevel, int endLevel)
        {
            QuadCell newQTreeCell = new QuadCell(location, boudingBox);
            if (!currentLevelCellsSet.Keys.Contains(currentLevel))
            {
                Collection<QuadCell> qTreeCells = new Collection<QuadCell> { newQTreeCell };
                currentLevelCellsSet.Add(currentLevel, qTreeCells);
            }
            else
            {
                currentLevelCellsSet[currentLevel].Add(newQTreeCell);
            }
            if (currentLevel < endLevel)
            {
                currentLevel++;
                GetCells(location + "1", GetUpperLeftQuater(boudingBox), currentLevelCellsSet, currentLevel, endLevel);
                GetCells(location + "2", GetUpperRightQuater(boudingBox), currentLevelCellsSet, currentLevel, endLevel);
                GetCells(location + "3", GetLowerLeftQuater(boudingBox), currentLevelCellsSet, currentLevel, endLevel);
                GetCells(location + "4", GetLowerRightQuater(boudingBox), currentLevelCellsSet, currentLevel, endLevel);
            }

            return currentLevelCellsSet;
        }

        public static RectangleShape GetUpperLeftQuater(RectangleShape boundingBox)
        {
            PointShape uppperLeft = new PointShape(boundingBox.UpperLeftPoint.X, boundingBox.UpperLeftPoint.Y);
            PointShape lowerRight = new PointShape((boundingBox.UpperLeftPoint.X + boundingBox.UpperRightPoint.X) / 2, (boundingBox.LowerLeftPoint.Y + boundingBox.UpperRightPoint.Y) / 2);
            return new RectangleShape(uppperLeft, lowerRight);
        }

        public static RectangleShape GetUpperRightQuater(RectangleShape boundingBox)
        {
            PointShape uppperLeft = new PointShape((boundingBox.UpperLeftPoint.X + boundingBox.UpperRightPoint.X) / 2, boundingBox.UpperLeftPoint.Y);
            PointShape lowerRight = new PointShape(boundingBox.UpperRightPoint.X, (boundingBox.UpperRightPoint.Y + boundingBox.LowerRightPoint.Y) / 2);
            return new RectangleShape(uppperLeft, lowerRight);
        }

        public static RectangleShape GetLowerLeftQuater(RectangleShape boundingBox)
        {
            PointShape uppperLeft = new PointShape(boundingBox.UpperLeftPoint.X, (boundingBox.UpperLeftPoint.Y + boundingBox.LowerLeftPoint.Y) / 2);
            PointShape lowerRight = new PointShape((boundingBox.LowerLeftPoint.X + boundingBox.LowerRightPoint.X) / 2, boundingBox.LowerLeftPoint.Y);
            return new RectangleShape(uppperLeft, lowerRight);
        }

        public static RectangleShape GetLowerRightQuater(RectangleShape boundingBox)
        {
            PointShape uppperLeft = new PointShape((boundingBox.UpperLeftPoint.X + boundingBox.UpperRightPoint.X) / 2, (boundingBox.LowerLeftPoint.Y + boundingBox.UpperRightPoint.Y) / 2);
            PointShape lowerRight = new PointShape(boundingBox.LowerRightPoint.X, boundingBox.LowerRightPoint.Y);
            return new RectangleShape(uppperLeft, lowerRight);
        }
    }
}
