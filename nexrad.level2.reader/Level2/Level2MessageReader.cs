using Autofac;
using nexrad.models;
using nexrad.reader.Level2.IndividualMessages;
using System.Collections.Generic;
using System.Linq;

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

        public RecordMessage ReadRecord(byte[] fileData, int offset)
        {
            if(offset == 325912) { int b = 11; }
            _byteReader.Seek(offset);
            _byteReader.Skip(Settings.CTM_HEADER_SIZE);

            var message = _messageHeaderReader.ReadHeader(fileData);

            switch (message.MessageType)
            {
                case 31:
                    List<RecordMessage> recordMessages = new List<RecordMessage>();

                    var messageRecord = _message31Reader.ReadMessage31(fileData);
                    message.Record = messageRecord;

                    var dataBlockPointers = _message31Reader.ReadDataBlockPointers(fileData);

                    message.Record.DataBlocks = dataBlockPointers;
                    var test1 = offset + dataBlockPointers.DBP1;
                    var test2 = offset + dataBlockPointers.DBP2;
                    var test3 = offset + dataBlockPointers.DBP3;
                    var test4 = offset + dataBlockPointers.DBP4;

                    var volumeData = _message31Reader.ParseVolumeData(fileData, test1);

                    if(_byteReader.Offset == 326049) { int xxx = 1; }

                    var elevationData = _message31Reader.ParseElevationData(fileData, test2);
                    var radialData = _message31Reader.ParseRadialData(fileData);

                    message.Record.VolumeData = volumeData;
                    message.Record.ElevationData = elevationData;
                    message.Record.RadialData = radialData;

                    #region Getting Individual Moment Data - the cool bit...

                    var reflectivityData = _message31Reader.ParseMomentData(fileData);
                    var reflectivityMomentData = _message31Reader.ParseReflectivityMomentData(fileData, reflectivityData.Offset, reflectivityData.Scale);
                    reflectivityData.MomentDataValues = reflectivityMomentData;

                    var velocityData = _message31Reader.ParseMomentData(fileData);
                    var velocityMomentData = _message31Reader.ParseVelocityMomentData(fileData, velocityData.Offset, velocityData.Scale);
                    velocityData.MomentDataValues = velocityMomentData;

                    var spectrumWidthData = _message31Reader.ParseMomentData(fileData);
                    var spectrumWidthMomentData = _message31Reader.ParseSpectrumWidthMomentData(fileData, spectrumWidthData.Offset, spectrumWidthData.Scale);
                    spectrumWidthData.MomentDataValues = spectrumWidthMomentData;

                    var differentialReflectivityData = _message31Reader.ParseMomentData(fileData);
                    var differentialReflectivityMomentData = _message31Reader.ParseDifferentialReflectivityMomentData(fileData, differentialReflectivityData.Offset, differentialReflectivityData.Scale);
                    differentialReflectivityData.MomentDataValues = differentialReflectivityMomentData;

                    var differentialPhaseData = _message31Reader.ParseMomentData(fileData);
                    var differentialPhaseMomentData = _message31Reader.ParseDifferentialPhaseMomentData(fileData, differentialPhaseData.Offset, differentialPhaseData.Scale);
                    differentialPhaseData.MomentDataValues = differentialPhaseMomentData;

                    #endregion

                    message.Record.ReflectivityData = reflectivityData;
                    message.Record.VelocityData = velocityData;
                    message.Record.SpectrumData = spectrumWidthData;
                    message.Record.ZDRData = differentialReflectivityData;
                    // PHI data coming soon...
                    message.Record.RhoData = differentialPhaseData;

                    break;
                default:
                    break;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            int a = 1;

            return message;
        }
    }
}
