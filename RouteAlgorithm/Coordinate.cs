
namespace RouteAlgorithm
{
    public  class Coordinate
    {
        private float latitude;
        private float longitude;

        public Coordinate()
            :this(0.0f,0.0f)
        { }

        public Coordinate(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }

        public float Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Latitude:{0}, Longitude:{1}", Latitude, Longitude);
        }
    }
}
