using Autofac;
using nexrad.models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RecordReader : ILevel2RecordReader
    {
        private int RecordNumber = 0;
        private int Offset = 0;
        private int VariableMessageOffset = 0;
        private bool IsEndOfFile = false;

        private byte[] FileData = null;

        private readonly ILevel2MessageReader _level2MessageReader;
        private readonly IByteReader _byteReader;
        List<int> data = new List<int>();

        public Level2RecordReader(ILevel2MessageReader level2MessageReader, IByteReader byteReader)
        {
            _level2MessageReader = level2MessageReader;
            _byteReader = byteReader;
        }

        public void LoadFile(string fileName)
        {
            FileData = File.ReadAllBytes(fileName);
        }

        public IList<GroupedMomentData> Read()
        {
            List<RecordMessage> recordMessages = new List<RecordMessage>();

            while (!IsEndOfFile)
            {
                if(RecordNumber == 134) { int b = 1; }
                Offset = RecordNumber * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + VariableMessageOffset;

                if (Offset >= GetLength()) break;

                data.Add(_byteReader.Offset);
                var message = _level2MessageReader.ReadRecord(FileData, Offset);

                if (message != null)
                {
                    if (message.Record != null)
                    {
                        if (message.Record.ReflectivityData != null) recordMessages.Add(message);
                        if (message.Record.VelocityData != null) recordMessages.Add(message);
                        if (message.Record.SpectrumData != null) recordMessages.Add(message);
                        if (message.Record.ZDRData != null) recordMessages.Add(message);
                        if (message.Record.PHIData != null) recordMessages.Add(message);
                        if (message.Record.RhoData != null) recordMessages.Add(message);
                    }

                    RecordNumber++;
                }
                else
                {
                    IsEndOfFile = true;
                }
            }

            var orderedResults = GroupAndSortData(recordMessages);

            return orderedResults;
        }

        public int GetLength()
        {
            return FileData.Length;
        }

        private List<GroupedMomentData> GroupAndSortData(List<RecordMessage> momentData)
        {
            var groups = new List<GroupedMomentData>();

            foreach (var item in momentData)
            {
                var elevationNumber = item.Record.ElevationNumber;

                var group = groups.SingleOrDefault(x => x.ElevationNumber == elevationNumber);

                if (group == null)
                {
                    group = new GroupedMomentData() { ElevationNumber = elevationNumber };
                    group.RecordMessages.Add(item);

                    groups.Add(group);
                }
                else
                {
                    groups.Where(x => x.ElevationNumber == elevationNumber).SingleOrDefault().RecordMessages.Add(item);
                }
            }

            // Just to make sure.
            return groups.OrderBy(x => x.ElevationNumber).ToList();
        }
    }
}
