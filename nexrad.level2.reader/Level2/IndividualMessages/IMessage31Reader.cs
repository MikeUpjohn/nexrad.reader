using nexrad.models;
using System.Collections.Generic;

namespace nexrad.reader.Level2.IndividualMessages
{
    public interface IMessage31Reader
    {
        RecordMessageRecord ReadMessage31(byte[] fileData);
        RecordMessageRecordDataBlock ReadDataBlockPointers(byte[] fileData);
    }
}
