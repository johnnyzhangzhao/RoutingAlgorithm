using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    [Serializable]
    internal class QTreeSpatialIndex : SpatialIndex
    {
        private static readonly object sync = new object();
        private Dictionary<string, QuadTreeNode> qtree;
        private RectangleShape maxExtent;
        private int maxLevel;

        protected QTreeSpatialIndex()
        {
        }

        public QTreeSpatialIndex(RectangleShape maxExtent)
        {
            this.qtree = new Dictionary<string, QuadTreeNode>();
            this.maxExtent = maxExtent;
            this.maxLevel = 0;
        }

        protected override bool IsOpenCore
        {
            get
            {
                return true;
            }
            set
            {
                base.IsOpenCore = value;
            }
        }

        protected override void AddCore(Feature feature)
        {
            int level = QTreeHelper.GetAppropriateLevel(maxExtent, feature.GetBoundingBox());
            string location = QTreeHelper.GetLocation(maxExtent, feature.GetShape(), level);
            if (!string.IsNullOrEmpty(location))
            {
                QuadCell cell = QTreeHelper.GetCellByLocation(maxExtent, location);
                lock (sync)
                {
                    if (!qtree.ContainsKey(location))
                    {
                        lock (sync)
                        {
                            QuadTreeNode node = new QuadTreeNode(cell.Location, cell.BoundingBox, feature.Id);
                            qtree.Add(location, node);
                        }
                        if (maxLevel < level)
                        {
                            maxLevel = level;
                        }
                    }
                }
            }
        }

        protected override Collection<string> GetFeatureIdsIntersectingBoundingBoxCore(RectangleShape boundingBox)
        {
            int level = QTreeHelper.GetAppropriateLevel(maxExtent, boundingBox);

            Collection<QuadTreeNode> intersectedNodes = GetIntersectedCells(boundingBox, level + 1);

            Collection<string> featureIds = new Collection<string>();
            foreach (QuadTreeNode node in intersectedNodes)
            {
                featureIds.Add(node.Id);
            }

            return featureIds;
        }

        protected override void DeleteCore(Feature feature)
        {
            int level = QTreeHelper.GetAppropriateLevel(maxExtent, feature.GetBoundingBox());
            string location = QTreeHelper.GetLocation(maxExtent, feature.GetShape(), level);
            if (!string.IsNullOrEmpty(location) && qtree.ContainsKey(location))
            {
                qtree.Remove(location);
            }
        }

        protected override int GetFeatureCountCore()
        {
            return qtree.Count;
        }

        private Collection<QuadTreeNode> GetIntersectedCells(RectangleShape boundingBox, int level)
        {

            Collection<QuadTreeNode> result = new Collection<QuadTreeNode>();

            string queriedBboxLocation = QTreeHelper.GetLocation(maxExtent, boundingBox, level);
            lock (sync)
            {
                foreach (string key in qtree.Keys)
                {
                    //QuadCell cell = QTreeHelper.GetCellByLocation(maxExtent, key);

                    if (key.StartsWith(queriedBboxLocation) && boundingBox.Intersects(QTreeHelper.GetCellByLocation(maxExtent, key).BoundingBox))
                    {
                        result.Add(qtree[key]);
                    }
                }
            }

            return result;
        } 

        private Collection<string> GetPossibleNodes(string location)
        {
            Collection<string> possibleNodes = new Collection<string>();
            foreach (string key in qtree.Keys)
            {
                if (key.StartsWith(location))
                {
                    possibleNodes.Add(key);
                }
            }

            return possibleNodes;
        }
    }
}
