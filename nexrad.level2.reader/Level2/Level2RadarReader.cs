using Autofac;
using nexrad.models;
using System.Collections.Generic;

namespace nexrad.level2.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RadarReader : ILevel2RadarReader
    {
        private readonly ILevel2RecordReader _level2RecordReader;

        int recordNumber = 1;
        bool endOfFile = false;

        public Level2RadarReader(ILevel2RecordReader level2RecordReader)
        {
            _level2RecordReader = level2RecordReader;
        }

        public void ReadLevel2RadarFile(string fileName)
        {
            _level2RecordReader.LoadFile(fileName);
            _level2RecordReader.SkipHeader();
        }
    }
}
