using System;
using System.Collections.Generic;
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

        public void WriteIndexNode(IndexAdjacentNode adjacentNode)
        {
            //indexOffsets.Add(adjacentNode.Id, indexFileStreamWriter.BaseStream.Position);

            //indexFileStreamWriter.Write(adjacentNode.AdjacentNodes.Count);
            //foreach (IndexAdjacentNode adjacentNode in adjacentNode.AdjacentNodes)
            //{
            //    indexFileStreamWriter.Write(adjacentNode.Id);
            //    indexFileStreamWriter.Write(adjacentNode.Cost);  
            //}
        }

        public void Close()
        {
            //// Update the Count and write the offset, and then close the stream.
            //indexFileStreamWriter.BaseStream.Seek(4, SeekOrigin.Begin);

            //foreach (var indexOffset in indexOffsets)
            //{
            //    indexFileStreamWriter.Write(indexOffset.Value);
            //}

            //indexFileStream.Close();
            //indexFileStream.Dispose();

            //indexFileStreamWriter.Close();
            //indexFileStreamWriter.Dispose();
        }
    }
}
