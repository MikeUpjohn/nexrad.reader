using nexrad.models;

namespace nexrad.level2.reader.Level2
{
    public interface ILevel2RecordReader
    {
        void LoadFile(string fileName);
        void SkipHeader();
        RecordMessage ReadMessageHeader();
    }
}
