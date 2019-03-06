using nexrad.models;
using System.Collections.Generic;

namespace nexrad.reader.Level2
{
    public interface ILevel2RecordReader
    {
        void LoadFile(string fileName);
        IList<RecordMessage> Read();
    }
}
