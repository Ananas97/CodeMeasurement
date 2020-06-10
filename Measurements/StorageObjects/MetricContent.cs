using System;
using System.Collections.Generic;
using System.IO;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class MetricContent
    {
        public int Begin, End;
        public string Name, FilePath;

        protected MetricContent(int begin, int end, string name, string filePath) 
        {
            this.Begin = begin;
            this.End = end;
            this.Name = name;
            this.FilePath = filePath;
        }

        public List<string> GetContent()
        {
            List<string> content = new List<string>();
            string[] fileContent = File.ReadAllLines(FilePath);

            int counter = 1;
            foreach (string line in fileContent)
            {
                if (counter >= Begin && counter <= End)
                {
                    content.Add(line);
                }
                counter++;
            }
            return content;
        }

        public void PrintContent()
        {
            string[] fileContent = File.ReadAllLines(FilePath);

            int counter = 1;
            foreach (string line in fileContent)
            {
                if (counter >= Begin && counter <= End)
                {
                    Console.WriteLine(line);
                }
                counter++;
            }
        }
    }
}
