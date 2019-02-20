using System;
using System.IO;
using System.Linq;

namespace nexrad.level2.reader
{
    public class FileReader
    {
        public readonly string _fileName;
        public int _currentByteCount = 0;
        public bool _isBigEndian = true;
        public byte[] _fileData;
        
        public FileReader(string fileName)
        {
            _fileName = fileName;
            _fileData = File.ReadAllBytes(_fileName);
        }

        public void Seek(int count)
        {
            _currentByteCount = count;
        }

        public void Skip(int count)
        {
            _currentByteCount += count;
        }

        public int GetFileLength()
        {
            return _fileData.Length;
        }

        public int GetCurrentByteLocation()
        {
            return _currentByteCount;
        }

        public short ReadShort()
        {
            byte[] data = new byte[2];
            Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, 2);
            CalculateEndian(data);

            _currentByteCount += 2;

            return BitConverter.ToInt16(data, 0);
        }

        public int ReadInteger()
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, 4);

            CalculateEndian(data);

            _currentByteCount += 4;

            return BitConverter.ToInt32(data, 0);
        }

        public byte ReadByte()
        {
            byte[] data = new byte[1];
            Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, 1);

            _currentByteCount++;

            return data.First();
        }

        public float ReadFloat()
        {
            byte[] data = new byte[4];
            Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, 4);

            CalculateEndian(data);

            _currentByteCount += 4;

            return BitConverter.ToSingle(data, 0);
        }

        public string ReadString(int countToRead)
        {
            byte[] data = new byte[countToRead];

            Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, countToRead);

            _currentByteCount += countToRead;

            return System.Text.Encoding.UTF8.GetString(data);
        }

        public byte[] Read(int countToRead)
        {
            byte[] data = null;

            if (countToRead > 1)
            {
                data = new byte[countToRead];
                Buffer.BlockCopy(_fileData, _currentByteCount, data, 0, countToRead);
                _currentByteCount += countToRead;
            }
            else
            {
                data = new byte[1];
                _currentByteCount++;
            }

            return data;
        }

        private byte[] CalculateEndian(byte[] data)
        {
            if (_isBigEndian)
            {
                Array.Reverse(data, 0, data.Length);
            }

            return data;
        }
    }
}
