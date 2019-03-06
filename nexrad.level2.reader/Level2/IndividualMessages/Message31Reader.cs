using System.Collections.Generic;
using Autofac;
using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    [InstancePerLifetimeScope]
    public class Message31Reader : IMessage31Reader
    {
        private readonly IByteReader _byteReader;

        public Message31Reader(IByteReader byteReader)
        {
            _byteReader = byteReader;
        }

        public RecordMessageRecord ReadMessage31(byte[] fileData)
        {
            RecordMessageRecord recordMessageRecord = new RecordMessageRecord()
            {
                Id = _byteReader.ReadString(fileData, 4),
                MSeconds = _byteReader.ReadInt(fileData),
                JulianDate = _byteReader.ReadShort(fileData),
                RadialNumber = _byteReader.ReadShort(fileData),
                Azimuth = _byteReader.ReadFloat(fileData),
                CompressIdx = _byteReader.ReadByte(fileData),
                Sp = _byteReader.ReadByte(fileData),
                RadialLength = _byteReader.ReadShort(fileData),
                ARS = _byteReader.ReadByte(fileData),
                RS = _byteReader.ReadByte(fileData),
                ElevationNumber = _byteReader.ReadByte(fileData),
                Cut = _byteReader.ReadByte(fileData),
                Elevation = _byteReader.ReadByte(fileData),
                RSBS = _byteReader.ReadByte(fileData),
                AIM = _byteReader.ReadByte(fileData),
                DCount = _byteReader.ReadShort(fileData),
            };

            return recordMessageRecord;
        }
        public RecordMessageRecordDataBlock ReadDataBlockPointers(byte[] fileData)
        {
            var dataBlockPointers = new RecordMessageRecordDataBlock()
            {
                DBP1 = _byteReader.ReadInt(fileData),
                DBP2 = _byteReader.ReadInt(fileData),
                DBP3 = _byteReader.ReadInt(fileData),
                DBP4 = _byteReader.ReadInt(fileData),
                DBP5 = _byteReader.ReadInt(fileData),
                DBP6 = _byteReader.ReadInt(fileData),
                DBP7 = _byteReader.ReadInt(fileData),
                DBP8 = _byteReader.ReadInt(fileData),
                DBP9 = _byteReader.ReadInt(fileData),
            };

            return dataBlockPointers;
        }
    }
}
