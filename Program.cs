using System;
using System.IO;

namespace CodeMeasurement
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            Measurement measurement1 = new LinesOfCode(path);
            Measurement measurement2 = new NumberOfClasses(path);

            Console.WriteLine("LOC:     " + measurement1.Result);
            Console.WriteLine("CLASSES: " + measurement2.Result);
        }
    }
}
