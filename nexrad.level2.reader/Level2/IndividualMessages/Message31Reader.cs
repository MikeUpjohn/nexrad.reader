using System.Collections.Generic;
using Autofac;
using Newtonsoft.Json;
using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    [InstancePerLifetimeScope]
    public class Message31Reader : IMessage31Reader
    {
        private readonly IByteReader _byteReader;
        private readonly IDataLogger _dataLogger;

        public Message31Reader(IByteReader byteReader, IDataLogger dataLogger)
        {
            _byteReader = byteReader;
            _dataLogger = dataLogger;
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
                CompressIdx = _byteReader.ReadByte(fileData).Value,
                Sp = _byteReader.ReadByte(fileData).Value,
                RadialLength = _byteReader.ReadShort(fileData),
                ARS = _byteReader.ReadByte(fileData).Value,
                RS = _byteReader.ReadByte(fileData).Value,
                ElevationNumber = _byteReader.ReadByte(fileData).Value,
                Cut = _byteReader.ReadByte(fileData).Value,
                Elevation = _byteReader.ReadFloat(fileData),
                RSBS = _byteReader.ReadByte(fileData).Value,
                AIM = _byteReader.ReadByte(fileData).Value,
                DCount = _byteReader.ReadShort(fileData),
            };

            _dataLogger.Log("Location 2 - End of RMR - at byte location " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(recordMessageRecord));

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


            if (_byteReader.Offset == 326008) { int dd = 1; }
            _dataLogger.Log("Location 3 - End of Data Block Pointers - at byte location " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(new List<int>() { dataBlockPointers.DBP1, dataBlockPointers.DBP2, dataBlockPointers.DBP3, dataBlockPointers.DBP4, dataBlockPointers.DBP5, dataBlockPointers.DBP6, dataBlockPointers.DBP7, dataBlockPointers.DBP8, dataBlockPointers.DBP9 }));

            return dataBlockPointers;
        }

        public VolumeData ParseVolumeData(byte[] fileData, int offset, int dbp)
        {
            _byteReader.Seek(offset + dbp + 28);

            var data = new VolumeData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                Name = _byteReader.ReadString(fileData, 3),
                Size = _byteReader.ReadShort(fileData),
                VersionMajor = _byteReader.ReadByte(fileData).Value,
                VersionMinor = _byteReader.ReadByte(fileData).Value,
                Latitude = _byteReader.ReadFloat(fileData),
                Longitude = _byteReader.ReadFloat(fileData),
                Elevation = _byteReader.ReadShort(fileData),
                FeedhornHeight = _byteReader.ReadByte(fileData).Value,
                Calibration = _byteReader.ReadFloat(fileData, 1),
                TxHorizontal = _byteReader.ReadFloat(fileData),
                TxVertical = _byteReader.ReadFloat(fileData),
                DifferentialReflectivity = _byteReader.ReadFloat(fileData),
                InitialSystemDifferentialPhase = _byteReader.ReadFloat(fileData),
                VolumeCoveragePattern = _byteReader.ReadByte(fileData).Value,
            };

            #region Pending Delete
            //_byteReader.Skip(2);
            #endregion

            _dataLogger.Log("Location 4 - End of Volume Data - at byte location - " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(data));

            return data;
        }

        public ElevationData ParseElevationData(byte[] fileData, int offset, int dbp)
        {
            _byteReader.Seek(offset + dbp + 28);
            if (_byteReader.Offset == 326049) { int y = 1; }
            var elevationData = new ElevationData()
            {
                BlockType = _byteReader.ReadString(fileData, 1),
                Name = _byteReader.ReadString(fileData, 3),
                Size = _byteReader.ReadShort(fileData),
                Atmos = _byteReader.ReadShort(fileData),
                Calibration = _byteReader.ReadFloat(fileData),
            };

            _dataLogger.Log("Location 5 - End of Elevation Data - at byte location - " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(elevationData));

            return elevationData;
        }

        public RadialData ParseRadialData(byte[] fileData, int offset, int dbp)
        {
            _byteReader.Seek(offset + dbp + 28);
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

            _dataLogger.Log("Location 6 - End of Radial Data - at byte location - " + _byteReader.Offset);
            _dataLogger.Log(JsonConvert.SerializeObject(radialData));

            return radialData;
        }

        public MomentData ParseMomentData(byte[] fileData, int offset, int dbp)
        {
            if (dbp > 0)
            {
                _byteReader.Seek(offset + dbp + 28);
                var data = new MomentData()
                {
                    BlockType = _byteReader.ReadString(fileData, 1),
                    MomentName = _byteReader.ReadString(fileData, 3),
                    GateCount = _byteReader.ReadShort(fileData, 4),
                    FirstGate = (_byteReader.ReadShort(fileData) / 1000),
                    GateSize = (_byteReader.ReadShort(fileData) / 1000),
                    RfThreshold = (_byteReader.ReadShort(fileData) / 10),
                    SnrThreshold = (_byteReader.ReadShort(fileData) / 1000),
                    ControlFlags = _byteReader.ReadByte(fileData).Value,
                    DataSize = _byteReader.ReadByte(fileData).Value,
                    Scale = _byteReader.ReadFloat(fileData),
                    Offset = _byteReader.ReadFloat(fileData),
                    DataOffset = dbp + 28,
                };

                _dataLogger.Log("Location 7 - End of Moment Header Data, before switch - at byte location " + _byteReader.Offset);
                _dataLogger.Log(JsonConvert.SerializeObject(data));

                return data;
            }

            return null;
        }

        public float[] ParseReflectivityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var reflectivityData = new List<float>();

            for (var i = 28; i <= 1867; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    reflectivityData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return reflectivityData.ToArray();
        }

        public float[] ParseVelocityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var velocityData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    velocityData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return velocityData.ToArray();
        }

        public float[] ParseSpectrumWidthMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var spectrumWidthData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    spectrumWidthData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return spectrumWidthData.ToArray();
        }

        public float[] ParseDifferentialReflectivityMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var differentialReflectivityData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    differentialReflectivityData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return differentialReflectivityData.ToArray();
        }

        public float[] ParseDifferentialPhaseMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var differentialPhaseData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    differentialPhaseData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return differentialPhaseData.ToArray();
        }

        public float[] ParseCorrelationCoefficientMomentData(byte[] fileData, float offset, float scale, int fileoffset, int dbp)
        {
            _byteReader.Seek(fileoffset + dbp + 28);
            _byteReader.Skip(28);

            var correlationCoefficientData = new List<float>();

            for (var i = 28; i <= 1227; i++)
            {
                var byteData = _byteReader.ReadByte(fileData);

                if (byteData != null)
                {
                    var data = (byteData.Value - offset) / scale;
                    correlationCoefficientData.Add(data);
                }
                else
                {
                    break;
                }
            }

            return correlationCoefficientData.ToArray();
        }
    }
}
