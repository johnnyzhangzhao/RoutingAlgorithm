using System;

namespace RouteAlgorithm
{
    internal class IndexFileReader
    {
        private string indexFileName;
        private ReadingCache readingCache;

        public IndexFileReader(string indexFileName)
        {
            this.indexFileName = indexFileName;
        }

        public void Open()
        {
            readingCache = new ReadingCache(indexFileName);

            int version = readingCache.ReadInt32();
            if (version != StreamSource.VERSION)
            {
                throw new NotSupportedException("The index file version is incorrect. Please check the file used is what you want or rebuild the index file.");
            }
        }

        public IndexNode ReadIndexNode(int nodeIndex)
        {
            int indexOffset = nodeIndex * 8;
            readingCache.SeekForward(indexOffset);  // move the the position where record the position of adjacent ids.

            long recordPosition = readingCache.ReadLong();
            readingCache.Seek(recordPosition);

            IndexNode indexNode = new IndexNode(nodeIndex);

            int adjacentCount = readingCache.ReadInt32();
            for (int i = 0; i < adjacentCount; i++)
            {
                int id = readingCache.ReadInt32();
                float cost = readingCache.ReadFloat();
                indexNode.AdjacentNodes.Add(new IndexAdjacentNode(id, cost));
            }

            return indexNode;
        }

        public void Close()
        {
            readingCache.Dispose();
        }
    }
}
