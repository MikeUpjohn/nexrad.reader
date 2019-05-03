using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nexrad.utilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var bytes = File.ReadAllBytes("F:\\TempDev\\logs\\final-output-1.json");

            File.WriteAllBytes("F:\\TempDev\\logs\\final-output-1-shortened.json", bytes.Take(10000000).ToArray());

        }
    }
}
