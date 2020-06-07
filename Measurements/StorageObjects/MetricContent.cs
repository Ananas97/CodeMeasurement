using System;
using System.Collections.Generic;
using System.IO;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class MetricContent
    {
        public int begin, end;
        public string name, filePath;

        protected MetricContent(int begin, int end, string name, string filePath) 
        {
            this.begin = begin;
            this.end = end;
            this.name = name;
            this.filePath = filePath;
        }

        public List<string> GetContent()
        {
            List<string> content = new List<string>();
            string[] fileContent = File.ReadAllLines(filePath);

            int counter = 1;
            foreach (string line in fileContent)
            {
                if (counter >= begin && counter <= end)
                {
                    content.Add(line);
                }
                counter++;
            }
            return content;
        }

        public void PrintContent()
        {
            string[] fileContent = File.ReadAllLines(filePath);

            int counter = 1;
            foreach (string line in fileContent)
            {
                if (counter >= begin && counter <= end)
                {
                    Console.WriteLine(line);
                }
                counter++;
            }
        }
    }
}
