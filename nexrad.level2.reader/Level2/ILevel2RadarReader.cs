namespace nexrad.level2.reader.Level2
{
    public interface ILevel2RadarReader
    {
        void ReadLevel2RadarFile(string fileName);
        // Call off to read Level 3 Radar File here, eventually
    }
}
