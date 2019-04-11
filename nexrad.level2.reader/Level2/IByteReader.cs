namespace nexrad.reader.Level2
{
    public interface IByteReader
    {
        bool IsBigEndian { get; set; }
        int Offset { get; set; }

        void Seek(int to);
        void Skip(int count);

        byte ReadByte(byte[] fileData);
        short ReadShort(byte[] fileData);
        short ReadShort(byte[] fileData, int skip);
        int ReadInt(byte[] fileData);
        float ReadFloat(byte[] fileData);
        float ReadFloat(byte[] fileData, int skip);
        string ReadString(byte[] fileData, int length);
    }
}
