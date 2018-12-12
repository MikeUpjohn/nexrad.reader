using System;

namespace nexrad_radar_data_reader
{
    public class Level2Parser
    {
        public RandomAccessFile rad;
        public int dbp;
        public int recordOffset;

        public Level2Parser(RandomAccessFile rad, int dbp, int recordOffset)
        {
            this.rad = rad;
            this.dbp = dbp;
            this.recordOffset = recordOffset;
        }

        public byte GetDataBlockByte(int skip)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.ReadByte();
        }

        public int GetDataBlockInteger(int skip)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.ReadInteger();
        }

        public byte[] GetDataBlockBytes(int skip, int size)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.Read(size);
        }

        public short GetDataBlockShort(int skip)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.ReadShort();
        }

        public float GetDataBlockFloat(int skip)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.ReadFloat();
        }

        public string GetDataBlockString(int skip, int size)
        {
            rad.Seek(dbp + recordOffset + Settings.MESSAGE_HEADER_SIZE);
            rad.Skip(skip);

            return rad.ReadString(size);
        }
    }
}
