
namespace RouteAlgorithm
{
    internal class IndexAdjacentNode
    {
        private int id;
        private float cost;
        private string featureId;

        private IndexAdjacentNode()
        { }

        public IndexAdjacentNode(int id)
            :this(id, 0)
        {
        }

        public IndexAdjacentNode(int id, float cost)
        {
            this.Id = id;
            this.Cost = cost;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public float Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public string FeatureId
        {
            get { return featureId; }
            set { featureId = value; }
        }
    }
}
