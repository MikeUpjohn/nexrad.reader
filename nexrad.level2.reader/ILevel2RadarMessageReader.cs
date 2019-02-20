using nexrad.models;

namespace nexrad.level2.reader
{
    public interface ILevel2RadarMessageReader
    {
        void SkipHeader();
        RecordMessage ReadMessageHeader();
    }
}
