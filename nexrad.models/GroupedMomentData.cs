using System.Collections.Generic;

namespace nexrad.models
{
    public class GroupedMomentData
    {
        public int ElevationNumber { get; set; }
        public List<RecordMessage> RecordMessages { get; set; } = new List<RecordMessage>();
    }
}
