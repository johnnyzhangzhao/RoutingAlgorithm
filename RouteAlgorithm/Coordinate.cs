namespace RouteAlgorithm
{
    public  class Coordinate
    {
        float latitude;
        float longitude;

        public Coordinate()
        {
        }

        public Coordinate( float latitude, float longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
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
            return string.Format("longitude:{0},latitude:{1}",longitude,latitude); 
        }
    }
}
