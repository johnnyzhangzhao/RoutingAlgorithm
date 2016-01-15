using System.Collections.Generic;
using System.IO;

namespace Test
{
    internal class IndexFileWriter
    {
        private string indexFileName;
        private Stream indexFileStream;
        private StreamWriter indexFileStreamWriter;

        private Dictionary<int, long> indexOffsets;

        public IndexFileWriter(string indexFileName)
        {
            this.indexFileName = indexFileName;
            this.indexOffsets = new Dictionary<int, long>();
        }

        public string IndexFileName
        {
            get { return indexFileName; }
        }

        public void Open(int recordCount)
        {
            indexFileStream = File.Open(indexFileName, FileMode.Open, FileAccess.ReadWrite);
            indexFileStreamWriter.Write(StreamSource.VERSION);
            indexFileStreamWriter.BaseStream.Seek(recordCount * 8, SeekOrigin.Current); // preserve for record offset.
        }

        public void WriteIndexNode(IndexNode indexNode)
        {
            indexOffsets.Add(indexNode.Id, indexFileStreamWriter.BaseStream.Position);

            indexFileStreamWriter.Write(indexNode.AdjacentNodes.Count);
            foreach (IndexAdjacentNode adjacentNode in indexNode.AdjacentNodes)
            {
                indexFileStreamWriter.Write(adjacentNode.Id);
                indexFileStreamWriter.Write(adjacentNode.Cost);
            }
        }

        public void Close()
        { 
            // Update the Count and write the offset, and then close the stream.
            indexFileStreamWriter.BaseStream.Seek(4, SeekOrigin.Begin);

            foreach (var indexOffset in indexOffsets)
            {
                indexFileStreamWriter.Write(indexOffset.Value);
            }

            indexFileStream.Close();
            indexFileStream.Dispose();

            indexFileStreamWriter.Close();
            indexFileStreamWriter.Dispose();
        }
    }
}
