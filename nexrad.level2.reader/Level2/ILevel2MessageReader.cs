using nexrad.models;

namespace nexrad.reader.Level2
{
    public interface ILevel2MessageReader
    {
        void SkipHeader();
        RecordMessage ReadRecord(byte[] fileData, int offset);
    }
}
