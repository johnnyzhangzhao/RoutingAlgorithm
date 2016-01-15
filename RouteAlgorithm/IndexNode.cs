using System.Collections.ObjectModel;

namespace Test
{
    internal class IndexNode
    {
        private int id;
        private Collection<IndexAdjacentNode> adjacentNodes;

        public IndexNode(int id)
        {
            this.Id = id;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Collection<IndexAdjacentNode> AdjacentNodes
        {
            get
            {
                if (adjacentNodes == null)
                {
                    adjacentNodes = new Collection<IndexAdjacentNode>();
                }
                return adjacentNodes;
            }
        }
    }
}
