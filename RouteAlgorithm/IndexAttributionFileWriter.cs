using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace RouteAlgorithm
{
    internal class IndexAttributionFileWriter
    {
        private string attributionFileName;
        private Stream attributionFileStream;
        private StreamWriter attributionFileStreamWriter;

        private Dictionary<int, long> indexOffsets;

        public IndexAttributionFileWriter(string attributionFileName)
        {
            this.attributionFileName = attributionFileName;
        }

        public string AttributionFileName
        {
            get { return attributionFileName; }
        }

        public void Open(int recordCount)
        {
            attributionFileStream = File.Open(AttributionFileName, FileMode.Open, FileAccess.ReadWrite);
            attributionFileStreamWriter = new StreamWriter(attributionFileStream);
            attributionFileStreamWriter.Write(StreamSource.VERSION);
            attributionFileStreamWriter.BaseStream.Seek(recordCount * 8, SeekOrigin.Current); // preserve for record offset.
        }

        public void WriteIndexNode(IndexNode indexNode)
        {
            indexOffsets.Add(indexNode.Id, attributionFileStreamWriter.BaseStream.Position);

            attributionFileStreamWriter.Write(indexNode.AdjacentNodes.Count);

            Collection<int> recordOffsets = new Collection<int>();

            int currentOffset = indexNode.AdjacentNodes.Count * 8;
            int currentFeatureIdLength = 0;
            foreach (IndexAdjacentNode adjacentNode in indexNode.AdjacentNodes)
            {
                attributionFileStreamWriter.Write(adjacentNode.Id);
                attributionFileStreamWriter.Write(currentOffset + currentFeatureIdLength);

                currentFeatureIdLength += adjacentNode.FeatureId.Length;
            }

            foreach (IndexAdjacentNode adjacentNode in indexNode.AdjacentNodes)
            {
                //attributionFileStreamWriter.Write(adjacentNode.FeatureId.Length);
                attributionFileStreamWriter.Write(adjacentNode.FeatureId);
            }
        }

        public void Close()
        {
            // Update the Count and write the offset, and then close the stream.
            attributionFileStreamWriter.BaseStream.Seek(4, SeekOrigin.Begin);

            foreach (var indexOffset in indexOffsets)
            {
                attributionFileStreamWriter.Write(indexOffset.Value);
            }

            attributionFileStream.Close();
            attributionFileStream.Dispose();

            attributionFileStreamWriter.Close();
            attributionFileStreamWriter.Dispose();
        }
    }
}
