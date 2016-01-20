using System;
using System.Reflection;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    [Serializable]
    internal class QuadCell
    {
        [Obfuscation(Exclude = true)]
        private string location;
        [Obfuscation(Exclude = true)]
        private RectangleShape boundingBox;

        public QuadCell()
            : this(string.Empty, new RectangleShape())
        { }

        public QuadCell(string location, RectangleShape boundingBox)
        {
            this.location = location;
            this.boundingBox = boundingBox;
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public RectangleShape BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }
    }
}
