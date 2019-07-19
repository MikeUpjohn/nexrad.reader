using nexrad.models;
using System.Collections.Generic;

namespace nexrad.reader.Level2
{
    public interface ILevel2RadarReader
    {
        List<GroupedMomentData> RunLevel2Radar(string fileName);
        // Call off to read Level 3 Radar File here, eventually
    }
}
