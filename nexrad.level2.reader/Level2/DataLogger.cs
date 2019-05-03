using Autofac;
using System;
using System.IO;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class DataLogger : IDataLogger
    {
        private bool isOn = false;

        public void Log(string text)
        {
            if (isOn)
            {
                File.AppendAllText("F:/TempDev/logs/radar-v2-log.txt", text + Environment.NewLine);
            }
        }
    }
}
