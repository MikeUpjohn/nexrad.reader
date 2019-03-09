using Autofac;
using System;
using System.IO;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class DataLogger : IDataLogger
    {
        public void Log(string text)
        {
            File.AppendAllText("F:/TempDev/logs/radar-v2-log.txt", text + Environment.NewLine);
        }
    }
}
