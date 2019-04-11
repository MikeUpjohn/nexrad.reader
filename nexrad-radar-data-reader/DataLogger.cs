using System;
using System.IO;

namespace nexrad_radar_data_reader
{
    public static class DataLogger
    {
        public static bool isLoggingOn = false;

        public static void Log(string text)
        {
            if (isLoggingOn)
            {
                File.AppendAllText("F:/TempDev/logs/radar-v1-log.txt", text + Environment.NewLine);
            }
        }
    }
}
