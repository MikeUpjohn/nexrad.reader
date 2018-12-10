using System;
using System.IO;
using System.Linq;

namespace nexrad_radar_data_reader
{
    public class RandomAccessFile
    {
        public int offset = 0;
        public byte[] buffer = null;
        public bool isBigEndian = false;

        public RandomAccessFile(string file)
        {
            buffer = File.ReadAllBytes(file);
        }

        public int GetLength()
        {
            return buffer.Length;
        }

        public void SetEndianOrder(int endian)
        {
            if (endian < 0) return;

            isBigEndian = (endian == Settings.BIG_ENDIAN);
        }

        public void Skip(int skip)
        {
            offset += skip;
        }

        public void Seek(int to)
        {
            offset = to;
        }

        public short ReadShort()
        {
            byte[] data = new byte[2];
            Buffer.BlockCopy(buffer, offset, data, 0, 2);

            if (isBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            offset += 2;

            return BitConverter.ToInt16(data, 0);
        }

        public int ReadInteger()
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(buffer, offset, data, 0, 4);

            if (isBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            offset += 4;

            return BitConverter.ToInt32(data, 0);
        }

        public byte ReadByte()
        {
            byte[] data = new byte[1];
            Buffer.BlockCopy(buffer, offset, data, 0, 1);

            offset++;

            return data.First();
        }

        public float ReadFloat()
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(buffer, offset, data, 0, 4);

            if (isBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            offset += 4;

            return BitConverter.ToSingle(data, 0);
        }

        public string ReadString(int countToRead)
        {
            byte[] data = new byte[countToRead];

            Buffer.BlockCopy(buffer, offset, data, 0, countToRead);

            offset += countToRead;

            return System.Text.Encoding.UTF8.GetString(data);
        }

        public byte[] Read(int countToRead)
        {
            byte[] data = null;

            if (countToRead > 1)
            {
                data = new byte[countToRead];
                Buffer.BlockCopy(buffer, offset, data, 0, countToRead);
                offset += countToRead;
            }
            else
            {
                data = new byte[1];
                offset++;
            }

            return data;
        }
    }
}
