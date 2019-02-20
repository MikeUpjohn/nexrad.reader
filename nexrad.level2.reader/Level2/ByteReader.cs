using Autofac;

namespace nexrad.level2.reader.Level2
{
    [InstancePerLifetimeScope]
    public class ByteReader : IByteReader
    {
        public bool IsBigEndian { get; set; }
    }
}
