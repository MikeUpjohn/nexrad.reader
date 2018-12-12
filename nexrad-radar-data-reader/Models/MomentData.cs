namespace nexrad_radar_data_reader.Models
{
    public class MomentData
    {
        public short GateCount { get; set; }
        public float FirstGate { get; set; }
        public float GateSize { get; set; }
        public float RfThreshold { get; set; }
        public float SnrThreshold { get; set; }
        public byte DataSize { get; set; }
        public float Scale { get; set; }
        public float Offset { get; set; }
        public int DataOffset{ get; set; }
        public float[] MomentDataValues { get; set; } // The big daddy...
    }
}
