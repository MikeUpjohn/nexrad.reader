using Autofac;
using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    [InstancePerLifetimeScope]
    public class MessageHeaderReader : IMessageHeaderReader
    {
        private readonly IByteReader _byteReader;

        public MessageHeaderReader(IByteReader byteReader)
        {
            _byteReader = byteReader;
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

            return record;
        }
    }
}
