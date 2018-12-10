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
    }

    public class RecordMessageRecord
    {
        public string Id { get; set; }
        public int MSeconds { get; set; }
        public short JulianDate { get; set; }
        public short RadialNumber { get; set; }
        public float Azimuth { get; set; }
        public byte CompressIdx { get; set; }
        public byte Spare { get; set; }
        public short RadialLength { get; set; }
        public byte AzimuthResolutionSpacing { get; set; }
        public byte RadiulStatus { get; set; }
        public byte ElevationNumber { get; set; }
        public byte CutSectorNumber { get; set; }
        public float ElevationAngle { get; set; }
        public byte RadialSpotBlankingStatus { get; set; }
        public byte AzimuthIndexMode { get; set; }
    }
}
