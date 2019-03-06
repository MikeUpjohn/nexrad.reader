using Autofac;
using nexrad.models;
using System.Collections.Generic;
using System.IO;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RecordReader : ILevel2RecordReader
    {
        private int RecordNumber = 0;
        private int Offset = 0;
        private int VariableMessageOffset = 0;
        private bool IsEndOfFile = false;

        private byte[] FileData = null;

        private readonly ILevel2MessageReader _level2MessageReader;
        private readonly IByteReader _byteReader;

        public Level2RecordReader(ILevel2MessageReader level2MessageReader, IByteReader byteReader)
        {
            _level2MessageReader = level2MessageReader;
            _byteReader = byteReader;
        }

        public void LoadFile(string fileName)
        {
            FileData = File.ReadAllBytes(fileName);
        }

        public IList<RecordMessage> Read()
        {
            while (!IsEndOfFile)
            {
                Offset = RecordNumber * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + VariableMessageOffset;

                if (Offset >= GetLength()) break;

                _level2MessageReader.ReadRecord(FileData, Offset); // return something here...

                RecordNumber++;
            }

            return new List<RecordMessage>();
        }

        public int GetLength()
        {
            return FileData.Length;
        }
    }
}
