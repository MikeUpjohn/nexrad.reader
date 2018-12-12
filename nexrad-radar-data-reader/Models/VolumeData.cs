namespace nexrad_radar_data_reader.Models
{
    public class VolumeData
    {
        public string BlockType { get; set; }
        public string Name { get; set; }
        public short Size { get; set; }
        public byte VersionMajor { get; set; }
        public byte VersionMinor { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public short Elevation { get; set; }
        public byte FeedhornHeight { get; set; }
        public float Calibration { get; set; }
        public float TxHorizontal { get; set; }
        public float TxVertical { get; set; }
        public float DifferentialReflectivity { get; set; }
        public byte VolumeCoveragePattern { get; set; }
    }
}
