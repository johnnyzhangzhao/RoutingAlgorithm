using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    class Bounds
    {
        float minlat;
        float maxlat;
        float minlon;
        float maxlon;

        public float Minlat
        {
            get
            {
                return minlat;
            }

            set
            {
                minlat = value;
            }
        }

        public float Maxlat
        {
            get
            {
                return maxlat;
            }

            set
            {
                maxlat = value;
            }
        }

        public float Minlon
        {
            get
            {
                return minlon;
            }

            set
            {
                minlon = value;
            }
        }

        public float Maxlon
        {
            get
            {
                return maxlon;
            }

            set
            {
                maxlon = value;
            }
        }

        public override string ToString()
        {
            return "{"+Minlat+","+Minlon+","+Maxlat+","+Maxlon+"}";
        }
    }
}
