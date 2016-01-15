using System;
using System.IO;
using System.Text;

namespace Test
{
    internal class ReadingCache : IDisposable
    {
        private int cacheSize;
        private const int floatSize = 4;
        private const int shortSize = 2;
        private const int intSize = 4;
        private const int longSize = 8;
        private const int byteSize = 1;
        private byte[] cache;
        private long currentPosition;
        private long cacheStart;
        private long cacheEnd;

        private Stream reader;

        public ReadingCache(string pathFileName)
        {
            this.cacheSize = 1024;//cacheSize;
            reader = new FileStream(pathFileName, FileMode.Open, FileAccess.Read);
            cache = new byte[1024];
        }

        public ReadingCache(Stream stream)
        {
            this.cacheSize = 1024;//cacheSize;
            this.cache = new byte[1024];
            this.reader = stream;
        }

        public byte ReadByte()
        {
            UpdateCache(byteSize);
            byte value = cache[currentPosition - cacheStart];
            ++currentPosition;
            return value;
        }

        public float ReadFloat()
        {
            UpdateCache(floatSize);
            float value = BitConverter.ToSingle(cache, (int)(currentPosition - cacheStart));
            currentPosition += floatSize;
            return value;
        }

        public short ReadInt16()
        {
            UpdateCache(shortSize);
            short value = BitConverter.ToInt16(cache, (int)(currentPosition - cacheStart));
            currentPosition += shortSize;
            return value;
        }

        public ushort ReadUInt16()
        {
            UpdateCache(shortSize);
            ushort value = BitConverter.ToUInt16(cache, (int)(currentPosition - cacheStart));
            currentPosition += shortSize;
            return value;
        }

        public int ReadInt32()
        {
            UpdateCache(intSize);
            int value = BitConverter.ToInt32(cache, (int)(currentPosition - cacheStart));
            currentPosition += intSize;
            return value;
        }

        public long ReadLong()
        {
            UpdateCache(longSize);
            long value = BitConverter.ToInt64(cache, (int)(currentPosition - cacheStart));
            currentPosition += longSize;
            return value;
        }

        public uint ReadUInt32()
        {
            UpdateCache(intSize);
            uint value = BitConverter.ToUInt32(cache, (int)(currentPosition - cacheStart));
            currentPosition += intSize;
            return value;
        }

        public string ReadString(int byteLengthOfSting)
        {
            string value = null;
            if (byteLengthOfSting <= cacheSize)
            {
                UpdateCache(byteLengthOfSting);
                value = Encoding.Default.GetString(cache, (int)(currentPosition - cacheStart), byteLengthOfSting);
            }
            else
            {
                byte[] bytes = new byte[byteLengthOfSting];
                reader.Read(bytes, 0, byteLengthOfSting);
                value = Encoding.Default.GetString(bytes, 0, byteLengthOfSting);
            }
            currentPosition += byteLengthOfSting;

            return value;
        }

        public byte[] ReadBytes(int size, ref int positionOfReturnArray)
        {
            byte[] bytes = new byte[size];
            if (size <= cacheSize)
            {
                UpdateCache(size);
                positionOfReturnArray = (int)(currentPosition - cacheStart);
                Buffer.BlockCopy(cache, (int)(currentPosition - cacheStart), bytes, 0, size);
            }
            else
            {
                reader.Read(bytes, 0, size);
                positionOfReturnArray = 0;
            }
            currentPosition += size;

            return bytes;
        }

        private void UpdateCache(int valueSize)
        {
            if (currentPosition + valueSize > cacheEnd || currentPosition < cacheStart)
            {
                cacheStart = currentPosition;
                cacheEnd = currentPosition + cacheSize;
                reader.Seek(cacheStart, SeekOrigin.Begin);
                reader.Read(cache, 0, cacheSize);
            }
        }

        public void Seek(long position)
        {
            currentPosition = position;
        }

        public void SeekForward(int offset)
        {
            if (offset != 0)
            {
                currentPosition += offset;
            }
        }

        public void SeekBackward(int offset)
        {
            if (offset != 0)
            {
                currentPosition -= offset;
            }
        }

        public bool IsEndOf()
        {
            if (currentPosition >= reader.Length - 1)
            {
                return true;
            }
            else { return false; }
        }

        public int GetFileLength()
        {
            return (int)reader.Length;
        }

        public void Dispose()
        {
            reader.Dispose();
            cache = null;
            GC.SuppressFinalize(this);
        }
    }
}

