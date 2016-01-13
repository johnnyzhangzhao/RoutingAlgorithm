namespace RouteAlgorithm
{
    public  class Coordinate
    {
        int id;
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

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
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
            return "{" + Id + "," + Latitude + "," +Longitude+"}" ;
        }
    }
}
