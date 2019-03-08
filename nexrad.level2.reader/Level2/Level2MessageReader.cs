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

                    var reflectivityData = _message31Reader.ParseMomentData(fileData);
                    var reflectivityMomentData = _message31Reader.ParseReflectivityMomentData(fileData, reflectivityData.Offset, reflectivityData.Scale);

                    var velocityData = _message31Reader.ParseMomentData(fileData);
                    var velocityMomentData = _message31Reader.ParseVelocityMomentData(fileData, velocityData.Offset, velocityData.Scale);

                    var spectrumWidthData = _message31Reader.ParseMomentData(fileData);
                    var spectrumWidthMomentDataData = _message31Reader.ParseSpectrumWidthMomentData(fileData, spectrumWidthData.Offset, spectrumWidthData.Scale);

                    var differentialReflectivityData = _message31Reader.ParseMomentData(fileData);
                    var differentialReflectivityMomentData = _message31Reader.ParseDifferentialReflectivityMomentData(fileData, differentialReflectivityData.Offset, differentialReflectivityData.Scale);

                    var differentialPhaseData = _message31Reader.ParseMomentData(fileData);
                    var differentialPhaseMomentData = _message31Reader.ParseDifferentialPhaseMomentData(fileData, differentialPhaseData.Offset, differentialPhaseData.Scale);


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
