using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkGeo.MapSuite.Core;

namespace RouteAlgorithm
{
    internal class FlagedVertex
    {
        private Vertex vertex;
        private bool flag;

        public Vertex Vertex
        {
            get
            {
                return vertex;
            }

            set
            {
                vertex = value;
            }
        }

        public bool Flag
        {
            get
            {
                return flag;
            }

            set
            {
                flag = value;
            }
        }
    }
}
