namespace nexrad.reader.Level2
{
    public interface ILevel2RadarReader
    {
        void RunLevel2Radar(string fileName);
        // Call off to read Level 3 Radar File here, eventually
    }
}
