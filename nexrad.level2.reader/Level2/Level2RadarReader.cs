using Autofac;
using nexrad.models;
using System.Collections.Generic;
using System.Linq;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RadarReader : ILevel2RadarReader
    {
        private readonly ILevel2RecordReader _level2RecordReader;

        public Level2RadarReader(ILevel2RecordReader level2RecordReader)
        {
            _level2RecordReader = level2RecordReader;
        }

        public List<GroupedMomentData> RunLevel2Radar(string fileName)
        {
            _level2RecordReader.LoadFile(fileName);
            var data = _level2RecordReader.Read();

            return data.ToList();
        }
    }
}
