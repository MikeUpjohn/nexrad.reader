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
        private readonly IDataLogger _dataLogger;

        public Level2MessageReader(IByteReader byteReader, IMessageHeaderReader messageHeaderReader, IMessage31Reader message31Reader, IDataLogger dataLogger)
        {
            _byteReader = byteReader;
            _messageHeaderReader = messageHeaderReader;
            _message31Reader = message31Reader;
            _dataLogger = dataLogger;
        }

        public void SkipHeader()
        {
            _byteReader.Seek(Settings.CTM_HEADER_SIZE);
        }

        public RecordMessage ReadRecord(byte[] fileData, int offset)
        {
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

                    var volumeData = _message31Reader.ParseVolumeData(fileData);
                    var elevationData = _message31Reader.ParseElevationData(fileData);
                    var radialData = _message31Reader.ParseRadialData(fileData);

                    message.Record.VolumeData = volumeData;
                    message.Record.ElevationData = elevationData;
                    message.Record.RadialData = radialData;

                    #region Getting Individual Moment Data - the cool bit...

                    var reflectivityData = _message31Reader.ParseMomentData(fileData);
                    var reflectivityMomentData = _message31Reader.ParseReflectivityMomentData(fileData, reflectivityData.Offset, reflectivityData.Scale);
                    reflectivityData.MomentDataValues = reflectivityMomentData;

                    _dataLogger.Log("Location 7a - End of reflectivity data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + reflectivityData.MomentDataValues.Length + " values");

                    var velocityData = _message31Reader.ParseMomentData(fileData);
                    var velocityMomentData = _message31Reader.ParseVelocityMomentData(fileData, velocityData.Offset, velocityData.Scale);
                    velocityData.MomentDataValues = velocityMomentData;

                    _dataLogger.Log("Location 7b - End of reflectivity data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + velocityData.MomentDataValues.Length + " values");

                    var spectrumWidthData = _message31Reader.ParseMomentData(fileData);
                    var spectrumWidthMomentData = _message31Reader.ParseSpectrumWidthMomentData(fileData, spectrumWidthData.Offset, spectrumWidthData.Scale);
                    spectrumWidthData.MomentDataValues = spectrumWidthMomentData;

                    _dataLogger.Log("Location 7c - End of Spectrum Width data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + spectrumWidthData.MomentDataValues + " values");

                    var differentialReflectivityData = _message31Reader.ParseMomentData(fileData);
                    var differentialReflectivityMomentData = _message31Reader.ParseDifferentialReflectivityMomentData(fileData, differentialReflectivityData.Offset, differentialReflectivityData.Scale);
                    differentialReflectivityData.MomentDataValues = differentialReflectivityMomentData;

                    _dataLogger.Log("Location 7d - End of Differential Reflectivity data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + differentialReflectivityData.MomentDataValues + " values");

                    var differentialPhaseData = _message31Reader.ParseMomentData(fileData);
                    var differentialPhaseMomentData = _message31Reader.ParseDifferentialPhaseMomentData(fileData, differentialPhaseData.Offset, differentialPhaseData.Scale);
                    differentialPhaseData.MomentDataValues = differentialPhaseMomentData;

                    _dataLogger.Log("Location 7e - End of PHI Data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + differentialPhaseData.MomentDataValues);

                    var correlationCoefficientData = _message31Reader.ParseMomentData(fileData);
                    var correlationCoefficientMomentData = _message31Reader.ParseCorrelationCoefficientMomentData(fileData, correlationCoefficientData.Offset, correlationCoefficientData.Scale);
                    correlationCoefficientData.MomentDataValues = correlationCoefficientMomentData;

                    _dataLogger.Log("Location 7f - End of Correlation Coefficient data - at byte location " + _byteReader.Offset);
                    _dataLogger.Log("Moment Array has " + correlationCoefficientData.MomentDataValues.Length + " values");

                    #endregion

                    message.Record.ReflectivityData = reflectivityData;
                    message.Record.VelocityData = velocityData;
                    message.Record.SpectrumData = spectrumWidthData;
                    message.Record.ZDRData = differentialReflectivityData;
                    // PHI data coming soon...
                    message.Record.RhoData = differentialPhaseData;

                    break;
                default:

                    _dataLogger.Log("Location 8 - Not a Message31 message. Moving on to next message");
                    break;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
            int a = 1;

            return message;
        }
    }
}
