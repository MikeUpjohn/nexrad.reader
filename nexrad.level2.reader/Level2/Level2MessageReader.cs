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
                    message.Record = messageRecord;

                    var dataBlockPointers = _message31Reader.ReadDataBlockPointers(fileData);
                    message.Record.DataBlocks = dataBlockPointers;

                    var volumeData = _message31Reader.ParseVolumeData(fileData);
                    var elevationData = _message31Reader.ParseElevationData(fileData);
                    var radialData = _message31Reader.ParseRadialData(fileData);

                    message.Record.VolumeData = volumeData;
                    message.Record.ElevationData = elevationData;
                    message.Record.RadialData = radialData;

                    #region

                    var momentData = _message31Reader.ParseMomentData(fileData);
                    var reflectivityData = _message31Reader.ParseReflectivityMomentData(fileData, momentData.Offset, momentData.Scale);
                    var velocityMomentData = _message31Reader.ParseVelocityMomentData(fileData, momentData.Offset, momentData.Scale);
                    var spectrumWidthData = _message31Reader.ParseSpectrumWidthMomentData(fileData, momentData.Offset, momentData.Scale);
                    var differentialReflectivityData = _message31Reader.ParseDifferentialReflectivityMomentData(fileData, momentData.Offset, momentData.Scale);
                    var differentialPhaseData = _message31Reader.ParseDifferentialPhaseMomentData(fileData, momentData.Offset, momentData.Scale);

                    message.Record.ref

                    #endregion

                    int ga = 1;

                    break;
                default:
                    break;
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            int a = 1;
        }
    }
}
