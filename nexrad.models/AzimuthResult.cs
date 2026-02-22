using System.Collections.Generic;

namespace nexrad.models {
    public class AzimuthResult {
        public HashSet<int> AvailableElevationScans { get; set; } = new HashSet<int>();
        public Dictionary<string, List<float>> AzimuthData { get; set; } = new Dictionary<string, List<float>>();
    }
}
