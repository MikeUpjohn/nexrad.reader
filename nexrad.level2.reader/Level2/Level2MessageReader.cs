using Autofac;
using nexrad.models;
using nexrad.reader.Level2.IndividualMessages;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2MessageReader : ILevel2MessageReader
    {
        private readonly IByteReader _byteReader;
        private readonly IMessageHeaderReader _messageHeaderReader;
        private readonly IMessage31Reader _message31Reader;

        public Level2MessageReader(IByteReader byteReader, IMessageHeaderReader messageHeaderReader, IMessage31Reader message31Reader)
        {
            _byteReader = byteReader;
            _messageHeaderReader = messageHeaderReader;
            _message31Reader = message31Reader;
        }

        public void SkipHeader()
        {
            _byteReader.Seek(Settings.CTM_HEADER_SIZE);
        }

        public void ReadRecord(byte[] fileData, int offset)
        {
            _byteReader.Seek(offset);
            _byteReader.Skip(Settings.CTM_HEADER_SIZE);

            var message = _messageHeaderReader.ReadHeader(fileData);

            switch (message.MessageType)
            {
                case 31:
                    var messageRecord = _message31Reader.ReadMessage31(fileData);
                    var dataBlockPointers = _message31Reader.ReadDataBlockPointers(fileData);
                    break;
                default:
                    break;
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            int a = 1;
        }
    }
}
