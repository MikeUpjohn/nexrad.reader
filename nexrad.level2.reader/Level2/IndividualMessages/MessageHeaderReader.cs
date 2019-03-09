using Autofac;
using Newtonsoft.Json;
using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    [InstancePerLifetimeScope]
    public class MessageHeaderReader : IMessageHeaderReader
    {
        private readonly IByteReader _byteReader;
        private readonly IDataLogger _dataLogger;

        public MessageHeaderReader(IByteReader byteReader, IDataLogger dataLogger)
        {
            _byteReader = byteReader;
            _dataLogger = dataLogger;
        }

        public RecordMessage ReadHeader(byte[] fileData)
        {
            var record = new RecordMessage()
            {
                MessageSize = _byteReader.ReadShort(fileData),
                Channel = _byteReader.ReadByte(fileData),
                MessageType = _byteReader.ReadByte(fileData),
                IdSequence = _byteReader.ReadShort(fileData),
                MessageJulianDate = _byteReader.ReadShort(fileData),
                MessageMilliseconds = _byteReader.ReadInt(fileData),
                SegmentCount = _byteReader.ReadShort(fileData),
                SegmentNumber = _byteReader.ReadShort(fileData),
            };

            _dataLogger.Log("Location 1 - at byte location " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(record));

            return record;
        }
    }
}
