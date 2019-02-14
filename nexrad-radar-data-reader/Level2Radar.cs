using nexrad.models;
using nexrad_radar_data_reader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace nexrad_radar_data_reader
{
    public class Level2Radar
    {
        public int Elevation = 0;
        public int Scan = 0;

        public Level2Radar(string fileName)
        {
            ParseRadarData(fileName);
        }

        private void ParseRadarData(string fileName)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var raf = new RandomAccessFile(fileName);
            var data = new List<RecordMessage>();

            raf.SetEndianOrder(Settings.BIG_ENDIAN);
            raf.Seek(Settings.FILE_HEADER_SIZE);

            int messageOffset31 = 0;
            int recordNo = 0;
            bool endOfFile = false;
            List<RecordMessage> momentData = new List<RecordMessage>();

            while (true && !endOfFile)
            {
                var x = new Level2Record(raf, recordNo++, messageOffset31);

                var recordOffset = recordNo * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + messageOffset31;
                if (recordOffset >= raf.GetLength()) break;

                var message = x.GetRecord(raf);

                if (message != null)
                {
                    data.Add(message);

                    if (message.MessageType == 31)
                    {
                        messageOffset31 = messageOffset31 + (message.MessageSize * 2 + 12 - 2432);
                    }
                }
                else
                {
                    endOfFile = true;
                }

                if (message.MessageType != 1 && message.MessageType != 31) continue;

                int av = 1;

                if (message.Record.ReflectivityData != null) momentData.Add(message);
                if (message.Record.VelocityData != null) momentData.Add(message);
                if (message.Record.SpectrumData != null) momentData.Add(message);
                if (message.Record.ZDRData != null) momentData.Add(message);
                if (message.Record.PHIData != null) momentData.Add(message);
                if (message.Record.RhoData != null) momentData.Add(message);
            }

            var oute = GroupAndSortData(momentData);
            
            watch.Stop();

            var a = watch.ElapsedMilliseconds;
        }

        private List<GroupedMomentData> GroupAndSortData(List<RecordMessage> momentData)
        {
            var groups = new List<GroupedMomentData>();

            foreach (var item in momentData)
            {
                var elevationNumber = item.Record.ElevationNumber;

                var group = groups.SingleOrDefault(x => x.ElevationNumber == elevationNumber);

                if(group == null)
                {
                    group = new GroupedMomentData() { ElevationNumber = elevationNumber };
                    group.RecordMessages.Add(item);

                    groups.Add(group);
                }
                else
                {
                    groups.Where(x => x.ElevationNumber == elevationNumber).SingleOrDefault().RecordMessages.Add(item);
                }
            }

            // Just to make sure.
            return groups.OrderBy(x=>x.ElevationNumber).ToList();
        }
    }
}
