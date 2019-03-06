using nexrad.models;

namespace nexrad.reader.Level2.IndividualMessages
{
    public interface IMessageHeaderReader
    {
        RecordMessage ReadHeader(byte[] fileData);
    }
}
