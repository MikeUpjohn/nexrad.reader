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

            if (message.MessageType == 31)
            {
                RecordMessageRecord rmr = new RecordMessageRecord()
                {
                    Id = rad.ReadString(4),
                    MSeconds = rad.ReadInteger(),
                    JulianDate = rad.ReadShort(),
                    RadialNumber = rad.ReadShort(),
                    Azimuth = rad.ReadFloat(),
                    CompressIdx = rad.ReadByte(),
                    Sp = rad.ReadByte(),
                    RadialLength = rad.ReadShort(),
                    ARS = rad.ReadByte(),
                    RS = rad.ReadByte(),
                    ElevationNumber = rad.ReadByte(),
                    Cut = rad.ReadByte(),
                    Elevation = rad.ReadFloat(),
                    RSBS = rad.ReadByte(),
                    AIM = rad.ReadByte(),
                    DCount = rad.ReadShort(),
                };

                message.Record = rmr;

                RecordMessageRecordDataBlock rmrdb = new RecordMessageRecordDataBlock()
                {
                    DBP1 = rad.ReadInteger(),
                    DBP2 = rad.ReadInteger(),
                    DBP3 = rad.ReadInteger(),
                    DBP4 = rad.ReadInteger(),
                    DBP5 = rad.ReadInteger(),
                    DBP6 = rad.ReadInteger(),
                    DBP7 = rad.ReadInteger(),
                    DBP8 = rad.ReadInteger(),
                    DBP9 = rad.ReadInteger(),
                };

                message.Record.DataBlocks = rmrdb;
            }

            return message;
        }
    }
}
