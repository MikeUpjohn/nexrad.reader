using Autofac;
using System;
using System.Linq;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class ByteReader : IByteReader
    {
        public bool IsBigEndian { get; set; } = true;
        public int Offset { get; set; }

        #region Byte Manipulation Functions

        public void Seek(int to)
        {
            Offset = to;
        }

        public void Skip(int count)
        {
            Offset += count;
        }

        #endregion

        public byte ReadByte(byte[] fileData)
        {
            byte[] data = new byte[1];
            Buffer.BlockCopy(fileData, Offset, data, 0, 1);

            Offset++;

            return data.First();
        }

        public short ReadShort(byte[] fileData)
        {
            byte[] data = new byte[2];
            Buffer.BlockCopy(fileData, Offset, data, 0, 2);

            if (IsBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            Offset += 2;

            return BitConverter.ToInt16(data, 0);
        }

        public short ReadShort(byte[] fileData, int skip)
        {
            Skip(skip);

            byte[] data = new byte[2];
            Buffer.BlockCopy(fileData, Offset, data, 0, 2);

            if (IsBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            Offset += 2;

            return BitConverter.ToInt16(data, 0);
        }

        public int ReadInt(byte[] fileData)
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(fileData, Offset, data, 0, 4);

            if (IsBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            Offset += 4;

            return BitConverter.ToInt32(data, 0);
        }

        public float ReadFloat(byte[] fileData)
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(fileData, Offset, data, 0, 4);

            if (IsBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            Offset += 4;

            return BitConverter.ToSingle(data, 0);
        }

        public string ReadString(byte[] fileData, int length)
        {
            byte[] data = new byte[length];

            Buffer.BlockCopy(fileData, Offset, data, 0, length);

            Offset += length;

            return System.Text.Encoding.UTF8.GetString(data);
        }

    }
}
