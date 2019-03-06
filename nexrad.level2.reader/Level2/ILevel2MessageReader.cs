namespace nexrad.reader.Level2
{
    public interface ILevel2MessageReader
    {
        void SkipHeader();
        void ReadRecord(byte[] fileData, int offset);
    }
}
