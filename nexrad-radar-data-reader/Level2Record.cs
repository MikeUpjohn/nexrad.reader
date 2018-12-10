using nexrad_radar_data_reader.Models;
using System;

namespace nexrad_radar_data_reader
{
    public class Level2Record
    {
        public int recordOffset = 0;

        public Level2Record(RandomAccessFile rad, int recordNumber, int messageOffset31)
        {
            recordOffset = recordNumber * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + messageOffset31;

            if (recordOffset >= rad.GetLength()) return;
        }

        public RecordMessage GetRecord(RandomAccessFile rad)
        {
            if (recordOffset >= rad.GetLength()) return null;

            rad.Seek(recordOffset);
            rad.Skip(Settings.CTM_HEADER_SIZE);

            var message = new RecordMessage()
            {
                MessageSize = rad.ReadShort(),
                Channel = rad.ReadByte(),
                MessageType = rad.ReadByte(),
                IdSequence = rad.ReadShort(),
                MessageJulianDate = rad.ReadShort(),
                MessageMilliseconds = rad.ReadInteger(),
                SegmentCount = rad.ReadShort(),
                SegmentNumber = rad.ReadShort(),
            };

            if(message.MessageType == 31)
            {
                message.
            }

            return message;
        }
    }
}
