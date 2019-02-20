using Autofac;
using nexrad.models;
using System.IO;

namespace nexrad.level2.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RecordReader : ILevel2RecordReader
    {
        private byte[] FileData = null;
        private int Offset = 0;

        private readonly IByteReader _byteReader;

        public Level2RecordReader(IByteReader byteReader)
        {
            _byteReader = byteReader;
        }

        public void LoadFile(string fileName)
        {
            FileData = File.ReadAllBytes(fileName);
        }

        public void SkipHeader()
        {
            Offset += Settings.CTM_HEADER_SIZE;
        }

        public RecordMessage ReadMessageHeader()
        {
            throw new System.NotImplementedException();
        }

    }
}
