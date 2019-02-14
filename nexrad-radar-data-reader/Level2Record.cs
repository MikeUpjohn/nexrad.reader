using nexrad.models;
using nexrad_radar_data_reader.Models;
using System;
using System.Collections.Generic;

namespace nexrad_radar_data_reader
{
    public class Level2Record
    {
        public int recordOffset = 0;

        public Level2Record(RandomAccessFile rad, int recordNumber, int messageOffset31)
        {
            recordOffset = recordNumber * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + messageOffset31;

            if (recordOffset >= rad.GetLength()) return;
        }

        public RecordMessage GetRecord(RandomAccessFile rad)
        {
            if (recordOffset >= rad.GetLength()) return null;

            rad.Seek(recordOffset);
            rad.Skip(Settings.CTM_HEADER_SIZE);

            var message = new RecordMessage()
            {
                MessageSize = rad.ReadShort(),
                Channel = rad.ReadByte(),
                MessageType = rad.ReadByte(),
                IdSequence = rad.ReadShort(),
                MessageJulianDate = rad.ReadShort(),
                MessageMilliseconds = rad.ReadInteger(),
                SegmentCount = rad.ReadShort(),
                SegmentNumber = rad.ReadShort(),
            };

            if (message.MessageType == 31)
            {
                RecordMessageRecord rmr = new RecordMessageRecord()
                {
                    Id = rad.ReadString(4),
                    MSeconds = rad.ReadInteger(),
                    JulianDate = rad.ReadShort(),
                    RadialNumber = rad.ReadShort(),
                    Azimuth = rad.ReadFloat(),
                    CompressIdx = rad.ReadByte(),
                    Sp = rad.ReadByte(),
                    RadialLength = rad.ReadShort(),
                    ARS = rad.ReadByte(),
                    RS = rad.ReadByte(),
                    ElevationNumber = rad.ReadByte(),
                    Cut = rad.ReadByte(),
                    Elevation = rad.ReadFloat(),
                    RSBS = rad.ReadByte(),
                    AIM = rad.ReadByte(),
                    DCount = rad.ReadShort(),
                };

                message.Record = rmr;

                //RecordMessageRecordDataBlock rmrdb = new RecordMessageRecordDataBlock()
                //{
                int DBP1 = rad.ReadInteger();
                int DBP2 = rad.ReadInteger();
                int DBP3 = rad.ReadInteger();
                int DBP4 = rad.ReadInteger();
                int DBP5 = rad.ReadInteger();
                int DBP6 = rad.ReadInteger();
                int DBP7 = rad.ReadInteger();
                int DBP8 = rad.ReadInteger();
                int DBP9 = rad.ReadInteger();
                //};

                //message.Record.DataBlocks = rmrdb;

                ParseVolumeData(rad, rmr, DBP1);
                ParseElevationData(rad, rmr, DBP2);
                ParseRadialData(rad, rmr, DBP3);
                ParseMomentData(rad, rmr, DBP4, "REF");
                ParseMomentData(rad, rmr, DBP5, "VEL");
                ParseMomentData(rad, rmr, DBP6, "SW");
                ParseMomentData(rad, rmr, DBP7, "ZDR");
                ParseMomentData(rad, rmr, DBP8, "PHI");
                ParseMomentData(rad, rmr, DBP9, "RHO");

                message.Record = rmr;
            }

            return message;
        }

        public void ParseVolumeData(RandomAccessFile rad, RecordMessageRecord rmr, int dbp)
        {
            var parser = new Level2Parser(rad, dbp, recordOffset);
            var data = new VolumeData()
            {
                BlockType = parser.GetDataBlockString(0, 1),
                Name = parser.GetDataBlockString(1, 3),
                Size = parser.GetDataBlockShort(4),
                VersionMajor = parser.GetDataBlockByte(6),
                VersionMinor = parser.GetDataBlockByte(7),
                Latitude = parser.GetDataBlockFloat(8),
                Longitude = parser.GetDataBlockFloat(12),
                Elevation = parser.GetDataBlockShort(16),
                FeedhornHeight = parser.GetDataBlockByte(18),
                Calibration = parser.GetDataBlockFloat(20),
                TxHorizontal = parser.GetDataBlockFloat(24),
                TxVertical = parser.GetDataBlockFloat(28),
                DifferentialReflectivity = parser.GetDataBlockFloat(32),
                VolumeCoveragePattern = parser.GetDataBlockByte(40),
            };

            rmr.VolumeData = data;
        }

        public void ParseElevationData(RandomAccessFile rad, RecordMessageRecord rmr, int dbp)
        {
            var parser = new Level2Parser(rad, dbp, recordOffset);
            var data = new ElevationData()
            {
                BlockType = parser.GetDataBlockString(0, 1),
                Name = parser.GetDataBlockString(1, 3),
                Size = parser.GetDataBlockShort(4),
                Atmos = parser.GetDataBlockShort(6),
                Calibration = parser.GetDataBlockFloat(8),
            };

            rmr.ElevationData = data;
        }

        public void ParseRadialData(RandomAccessFile rad, RecordMessageRecord rmr, int dbp)
        {
            var parser = new Level2Parser(rad, dbp, recordOffset);
            var data = new RadialData()
            {
                BlockType = parser.GetDataBlockString(0, 1),
                Name = parser.GetDataBlockString(1, 3),
                Size = parser.GetDataBlockShort(4),
                UmambiguousRange = parser.GetDataBlockShort(6),
                HorizontalNoiseLevel = parser.GetDataBlockFloat(8),
                VerticalNoiseLevel = parser.GetDataBlockFloat(12),
                NyquistVelocity = parser.GetDataBlockShort(16),
            };

            rmr.RadialData = data;
        }

        public void ParseMomentData(RandomAccessFile rad, RecordMessageRecord rmr, int dbp, string type)
        {
            var parser = new Level2Parser(rad, dbp, recordOffset);
            var data = new MomentData()
            {
                GateCount = parser.GetDataBlockShort(8),
                FirstGate = (parser.GetDataBlockShort(10) / 1000),
                GateSize = (parser.GetDataBlockShort(12) / 1000),
                RfThreshold = (parser.GetDataBlockShort(14) / 10),
                SnrThreshold = (parser.GetDataBlockShort(16) / 1000),
                DataSize = parser.GetDataBlockByte(19),
                Scale = parser.GetDataBlockFloat(20),
                Offset = parser.GetDataBlockFloat(24),
                DataOffset = dbp + 28,
            };

            switch (type)
            {
                case "REF":
                    var reflectivityData = new List<float>();
                    for (int i = 28; i <= 1867; i++)
                    {
                        reflectivityData.Add((parser.GetDataBlockByte(i) - data.Offset) / data.Scale);
                    }

                    data.MomentDataValues = new float[reflectivityData.Count];
                    data.MomentDataValues = reflectivityData.ToArray();
                    rmr.ReflectivityData = data;
                    break;
                case "VEL":
                    var velocityData = new List<float>();
                    for(int i = 28; i <= 1227; i++)
                    {
                        velocityData.Add((parser.GetDataBlockByte(i) - data.Offset) / data.Scale);
                    }

                    data.MomentDataValues = new float[velocityData.Count];
                    data.MomentDataValues = velocityData.ToArray();
                    rmr.VelocityData = data;
                    break;
                case "SW":
                    var swData = new List<float>();
                    for(int i = 28; i <= 1227; i++)
                    {
                        swData.Add((parser.GetDataBlockByte(i) - data.Offset) / data.Scale);
                    }

                    data.MomentDataValues = new float[swData.Count];
                    data.MomentDataValues = swData.ToArray();
                    rmr.SpectrumData = data;
                    break;
                case "ZDR":
                    var zdrData = new List<float>();
                    for(int i = 28; i <= 1227; i++)
                    {
                        zdrData.Add((parser.GetDataBlockByte(i) - data.Offset) / data.Scale);
                    }

                    data.MomentDataValues = new float[zdrData.Count];
                    data.MomentDataValues = zdrData.ToArray();
                    rmr.ZDRData = data;
                    break;
                case "PHI":
                    // coming soon...
                    break;
                case "RHO":
                    var rhoData = new List<float>();
                    for(int i = 28; i <= 1227; i++)
                    {
                        rhoData.Add((parser.GetDataBlockByte(i) - data.Offset) / data.Scale);
                    }

                    data.MomentDataValues = new float[rhoData.Count];
                    data.MomentDataValues = rhoData.ToArray();
                    rmr.RhoData = data;
                    break;
            }
        }
    }
}
