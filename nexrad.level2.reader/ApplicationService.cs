using Autofac;
using nexrad.reader.Level2;

namespace nexrad.reader
{
    [InstancePerLifetimeScope]
    public class ApplicationService : IApplicationService
    {
        private readonly ILevel2RadarReader _level2RadarReader;

        public ApplicationService(ILevel2RadarReader level2RadarReader)
        {
            _level2RadarReader = level2RadarReader;
        }

        public void Run()
        {
            string fileName = "F:\\TempDev\\nexrad-radar-data-reader\\nexrad-radar-data-reader\\KAKQ20110504_000344_V03";
            _level2RadarReader.RunLevel2Radar(fileName);
        }
    }
}
