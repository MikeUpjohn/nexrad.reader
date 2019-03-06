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
        int ReadInt(byte[] fileData);
        float ReadFloat(byte[] fileData);
        string ReadString(byte[] fileData, int length);
    }
}
