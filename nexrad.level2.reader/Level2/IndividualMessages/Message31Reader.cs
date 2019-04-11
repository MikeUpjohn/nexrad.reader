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
                Elevation = _byteReader.ReadFloat(fileData),
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

        public VolumeData ParseVolumeData(byte[] fileData, int location)
        {
            _byteReader.Seek(location + 28);
            var data = new VolumeData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                Name = _byteReader.ReadString(fileData, 3),
                Size = _byteReader.ReadShort(fileData),
                VersionMajor = _byteReader.ReadByte(fileData),
                VersionMinor = _byteReader.ReadByte(fileData),
                Latitude = _byteReader.ReadFloat(fileData),
                Longitude = _byteReader.ReadFloat(fileData),
                Elevation = _byteReader.ReadShort(fileData),
                FeedhornHeight = _byteReader.ReadByte(fileData),
                Calibration = _byteReader.ReadFloat(fileData, 1),
                TxHorizontal = _byteReader.ReadFloat(fileData),
                TxVertical = _byteReader.ReadFloat(fileData),
                DifferentialReflectivity = _byteReader.ReadFloat(fileData),
                InitialSystemDifferentialPhase = _byteReader.ReadFloat(fileData),
                VolumeCoveragePattern = _byteReader.ReadByte(fileData),
            };
            
            return data;
        }

        public ElevationData ParseElevationData(byte[] fileData, int location)
        {
            _byteReader.Seek(location + 28);
            if (_byteReader.Offset == 326049) { int aaads = 1; }

            var elevationData = new ElevationData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                Name = _byteReader.ReadString(fileData, 3),
                Size = _byteReader.ReadShort(fileData),
                Atmos = _byteReader.ReadShort(fileData),
                Calibration = _byteReader.ReadFloat(fileData),
            };

            return elevationData;
        }

        public RadialData ParseRadialData(byte[] fileData)
        {
            var radialData = new RadialData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                Name = _byteReader.ReadString(fileData, 3),
                Size = _byteReader.ReadShort(fileData),
                UmambiguousRange = _byteReader.ReadShort(fileData),
                HorizontalNoiseLevel = _byteReader.ReadFloat(fileData),
                VerticalNoiseLevel = _byteReader.ReadFloat(fileData),
                NyquistVelocity = _byteReader.ReadShort(fileData),
            };

            _byteReader.Skip(2);

            return radialData;
        }

        public MomentData ParseMomentData(byte[] fileData)
        {
            var data = new MomentData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                MomentName = _byteReader.ReadString(fileData, 3),
                GateCount = _byteReader.ReadShort(fileData, 4),
                FirstGate = (_byteReader.ReadShort(fileData) / 1000),
                GateSize = (_byteReader.ReadShort(fileData) / 1000),
                RfThreshold = (_byteReader.ReadShort(fileData) / 10),
                SnrThreshold = (_byteReader.ReadShort(fileData) / 1000),
                ControlFlags = _byteReader.ReadByte(fileData),
                DataSize = _byteReader.ReadByte(fileData),
                Scale = _byteReader.ReadFloat(fileData),
                Offset = _byteReader.ReadFloat(fileData),
            };

            return data;
        }

        public float[] ParseReflectivityMomentData(byte[] fileData, float offset, float scale)
        {
            var reflectivityData = new List<float>();

            for (var i = 28; i <= 1867; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                reflectivityData.Add(data);
            }

            return reflectivityData.ToArray();
        }

        public float[] ParseVelocityMomentData(byte[] fileData, float offset, float scale)
        {
            var velocityData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                velocityData.Add(data);
            }

            return velocityData.ToArray();
        }

        public float[] ParseSpectrumWidthMomentData(byte[] fileData, float offset, float scale)
        {
            var spectrumWidthData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                spectrumWidthData.Add(data);
            }

            return spectrumWidthData.ToArray();
        }

        public float[] ParseDifferentialReflectivityMomentData(byte[] fileData, float offset, float scale)
        {
            var differentialReflectivityData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                differentialReflectivityData.Add(data);
            }

            return differentialReflectivityData.ToArray();
        }

        public float[] ParseDifferentialPhaseMomentData(byte[] fileData, float offset, float scale)
        {
            var differentialPhaseData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                differentialPhaseData.Add(data);
            }

            return differentialPhaseData.ToArray();
        }

        public float[] ParseCorrelationCoefficientMomentData(byte[] fileData, float offset, float scale)
        {
            var correlationCoefficientData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var data = (_byteReader.ReadByte(fileData) - offset) / scale;
                correlationCoefficientData.Add(data);
            }

            return correlationCoefficientData.ToArray();
        }
    }
}
