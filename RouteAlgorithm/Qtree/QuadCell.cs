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
        private ulong location;
        [Obfuscation(Exclude = true)]
        private RectangleShape boundingBox;

        public QuadCell()
            : this(0, new RectangleShape())
        { }

        public QuadCell(ulong location, RectangleShape boundingBox)
        {
            this.location = location;
            this.boundingBox = boundingBox;
        }

        public ulong Location
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
