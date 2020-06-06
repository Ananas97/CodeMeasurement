using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class FunctionMetric
    {
        public int NumberOfLines, NumberOfComments, NestedBlockDepth, begin, end;
        public string name, filePath;
        public List<string> content = new List<string>();

        public FunctionMetric(int begin, int end, string name, string filePath)
        {
            this.begin = begin;
            this.end = end;
            this.name = name;
            this.filePath = filePath;
        }
    }
}
