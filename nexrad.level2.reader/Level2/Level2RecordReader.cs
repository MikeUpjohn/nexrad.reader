﻿using Autofac;
using Newtonsoft.Json;
using nexrad.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace nexrad.reader.Level2
{
    [InstancePerLifetimeScope]
    public class Level2RecordReader : ILevel2RecordReader
    {
        #region Private Variables
        private int RecordNumber = 0;
        private int Offset = 0;
        private int VariableMessageOffset = 0;
        private bool IsEndOfFile = false;
        private byte[] FileData = null;
        #endregion

        private readonly ILevel2MessageReader _level2MessageReader;
        private readonly IByteReader _byteReader;

        public Level2RecordReader(ILevel2MessageReader level2MessageReader, IByteReader byteReader)
        {
            _level2MessageReader = level2MessageReader;
            _byteReader = byteReader;
        }

        public void LoadFile(string fileName)
        {
            using(WebClient client = new WebClient())
            {
                FileData = client.DownloadData(fileName);
            }
        }

        public IList<GroupedMomentData> Read()
        {
            List<RecordMessage> recordMessages = new List<RecordMessage>();

            while (!IsEndOfFile)
            {
                Offset = RecordNumber * Settings.RADAR_DATA_SIZE + Settings.FILE_HEADER_SIZE + VariableMessageOffset;

                if (Offset >= GetLength()) break;                
                var message = _level2MessageReader.ReadRecord(FileData, Offset);

                if (message.MessageType == 31)
                {
                    VariableMessageOffset = VariableMessageOffset + (message.MessageSize * 2 + 12 - 2432);
                }

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
