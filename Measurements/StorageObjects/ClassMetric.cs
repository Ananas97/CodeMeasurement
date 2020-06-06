using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class ClassMetric
    {
        public List<FunctionMetric> FunctionMetricList = new List<FunctionMetric>();
        public int NumberOfLines, NumberOfComments, NumberOfChildrens, DepthOfInheritance, WeightedMethods, begin, end;
        public string filePath, description, name;
        public List<string> content = new List<string>();

        public ClassMetric(int begin, int end, string name, string filePath)
        {
            this.begin = begin;
            this.end = end;
            this.name = name;
            this.filePath = filePath;
        }

        public void ShowContent()
        {
            Console.WriteLine("NAME: " + name);
            foreach (string line in content)
            {
                Console.WriteLine(line);
            }
        }
    }
}
