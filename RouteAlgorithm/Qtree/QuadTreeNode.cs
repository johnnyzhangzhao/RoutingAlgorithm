using System.Collections.ObjectModel;
using System.Reflection;
using System;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    [Serializable]
    internal class QuadTreeNode
    {
        [Obfuscation(Exclude = true)]
        private ulong location;
        [Obfuscation(Exclude = true)]
        private RectangleShape boundingBox;
        [Obfuscation(Exclude = true)]
        private string id;
        [Obfuscation(Exclude = true)]
        private Collection<QuadCell> children;

        public QuadTreeNode()
            : this(0, new RectangleShape(), string.Empty)
        { }

        public QuadTreeNode(string location, RectangleShape boundingBox)
            : this(0, new RectangleShape(), string.Empty)
        {
        }

        public QuadTreeNode(ulong location, RectangleShape boundingBox, string id)
        {
            this.location = location;
            this.boundingBox = boundingBox;
            this.id = id;
            this.children = new Collection<QuadCell>();
        }

        public ulong Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public Collection<QuadCell> Children
        {
            get { return children; }
        }

        public RectangleShape BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }
    }
}
