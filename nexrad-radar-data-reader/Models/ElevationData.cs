namespace nexrad_radar_data_reader.Models
{
    public class ElevationData
    {
        public string BlockType { get; set; }
        public string Name { get; set; }
        public short Size { get; set; }
        public short Atmos { get; set; }
        public float Calibration { get; set; }
    }
}
