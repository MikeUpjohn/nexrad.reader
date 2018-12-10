using nexrad_radar_data_reader.Models;
using System.Collections.Generic;
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
            var raf = new RandomAccessFile(fileName);
            var data = new List<RecordMessage>();

            raf.SetEndianOrder(Settings.BIG_ENDIAN);
            raf.Seek(Settings.FILE_HEADER_SIZE);

            int messageOffset31 = 0;
            int recordNo = 0;
            bool endOfFile = false;

            while (true && !endOfFile)
            {
                var x = new Level2Record(raf, recordNo++, messageOffset31);
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

                //if (message.MessageType != 1 && message.MessageType != 31) continue;
            }
            var interestingstuff = data.Where(x => x.MessageType == 31).ToList();

            int a = 1;
        }
    }
}
