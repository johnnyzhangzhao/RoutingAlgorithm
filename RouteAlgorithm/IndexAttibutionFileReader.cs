using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RouteAlgorithm
{
    internal class IndexAttibutionFileReader : IDisposable
    {
        private string attributionFileName;
        private BinaryReader attributionFileStreamReader;
        private bool isOpened;
        private long dataOffset;
        private int recordsCount;

        private Dictionary<int, long> indexOffsets;

        public IndexAttibutionFileReader(string attributionFileName)
        {
            this.attributionFileName = attributionFileName;
        }

        public void Open()
        {
            if(!File.Exists(attributionFileName))
            {
                throw new FileNotFoundException("The specified attribution file is not found");
            }

            attributionFileStreamReader = new BinaryReader(File.Open(attributionFileName, FileMode.Open));
            attributionFileStreamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            recordsCount = attributionFileStreamReader.ReadInt32();

            dataOffset = attributionFileStreamReader.ReadInt64();

            for (int i = 0; i < recordsCount; i++)
            {
                if (indexOffsets == null)
                {
                    indexOffsets = new Dictionary<int, long>();
                }

                indexOffsets.Add(i, attributionFileStreamReader.ReadInt64());
            }

            isOpened = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close()
        {
            if(isOpened)
            {
                attributionFileStreamReader.Close();
                isOpened = false;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            if(!isOpened)
            {
                attributionFileStreamReader.Dispose();
            }
        }

        public Dictionary<int, int> ReadRecordFromIndex(int index)
        {
            Dictionary<int, int> recordItems = null;
            attributionFileStreamReader.BaseStream.Seek(indexOffsets[index], SeekOrigin.Begin);
            int currentRecordItemsCount = attributionFileStreamReader.ReadInt32();

            for (int i = 0; i < currentRecordItemsCount; i++)
            {
                if(recordItems == null)
                {
                    recordItems = new Dictionary<int, int>();
                }

                recordItems.Add(attributionFileStreamReader.ReadInt32(), attributionFileStreamReader.ReadInt32());
            }

            return recordItems;
        }

        public string ReadRecordString(long recordOffset, int index, Dictionary<int, int> recordItems)
        {
            long offset = recordOffset + recordItems[index];
            attributionFileStreamReader.BaseStream.Seek(offset, SeekOrigin.Begin);

            return attributionFileStreamReader.ReadString();
        }
    }
}
