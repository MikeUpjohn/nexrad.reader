namespace nexrad.api.Models
{
    public class RadarQuery
    {
        public string RadarFile { get; set; }
        public int ElevationNumber { get; set; }
        public int? Scan { get; set; } = null;
    }
}