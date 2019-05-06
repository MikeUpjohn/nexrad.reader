namespace nexrad.api.Models
{
    public class RadarQuery
    {
        public string RadarFile { get; set; }
        public int ElevationNumber { get; set; } = 1;
        public int? Scan { get; set; } = null;
    }
}