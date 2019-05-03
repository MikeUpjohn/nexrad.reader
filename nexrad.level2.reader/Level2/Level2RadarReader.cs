using Autofac;

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

        public void RunLevel2Radar(string fileName)
        {
            _level2RecordReader.LoadFile(fileName);
            var data = _level2RecordReader.Read();
        }
    }
}
