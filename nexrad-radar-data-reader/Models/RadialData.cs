namespace nexrad_radar_data_reader.Models
{
    public class RadialData
    {
        public string BlockType { get; set; }
        public string Name { get; set; }
        public short Size { get; set; }
        public short UmambiguousRange { get; set; }
        public float HorizontalNoiseLevel { get; set; }
        public float VerticalNoiseLevel { get; set; }
        public short NyquistVelocity { get; set; }
    }
}
