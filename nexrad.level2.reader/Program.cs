using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nexrad.level2.reader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reader = new Level2RadarReader("F:\\TempDev\\nexrad-radar-data-reader\\nexrad-radar-data-reader\\KAKQ20110504_000344_V03");
            reader.ReadNexradLevel2RadarData();
        }
    }
}
