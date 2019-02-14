namespace nexrad_radar_data_reader.Models
{
    public class RecordMessage
    {
        public short MessageSize { get; set; }
        public byte Channel { get; set; }
        public byte MessageType { get; set; }
        public short IdSequence { get; set; }
        public short MessageJulianDate { get; set; }
        public int MessageMilliseconds { get; set; }
        public short SegmentCount { get; set; }
        public short SegmentNumber { get; set; }
        public RecordMessageRecord Record { get; set; }
    }

    public class RecordMessageRecord
    {
        public string Id { get; set; }
        public int MSeconds { get; set; }
        public short JulianDate { get; set; }
        public short RadialNumber { get; set; }
        public float Azimuth { get; set; }
        public byte CompressIdx { get; set; }
        public byte Sp { get; set; }
        public short RadialLength { get; set; }
        public byte ARS { get; set; }
        public byte RS { get; set; }
        public byte ElevationNumber { get; set; }
        public byte Cut { get; set; }
        public float Elevation { get; set; }
        public byte RSBS { get; set; }
        public byte AIM { get; set; }
        public short DCount { get; set; }
        //public RecordMessageRecordDataBlock DataBlocks { get; set; }
        public VolumeData VolumeData { get; set; }
        public ElevationData ElevationData { get; set; }
        public RadialData RadialData { get; set; }
        public MomentData ReflectivityData { get; set; }
        public MomentData VelocityData { get; set; }
        public MomentData SpectrumData { get; set; }
        public MomentData ZDRData { get; set; }
        public MomentData PHIData { get; set; }
        public MomentData RhoData { get; set; }
    }

    //public class RecordMessageRecordDataBlock
    //{
    //    public int DBP1 { get; set; }
    //    public int DBP2 { get; set; }
    //    public int DBP3 { get; set; }
    //    public int DBP4 { get; set; }
    //    public int DBP5 { get; set; }
    //    public int DBP6 { get; set; }
    //    public int DBP7 { get; set; }
    //    public int DBP8 { get; set; }
    //    public int DBP9 { get; set; }
    //}
}
