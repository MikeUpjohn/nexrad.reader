using System.Collections.Generic;

namespace nexrad_radar_data_reader.Models
{
    public class GroupedMomentData
    {
        public int ElevationNumber { get; set; }
        public List<RecordMessage> RecordMessages { get; set; } = new List<RecordMessage>();
    }
}
