using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class ClassMetric
    {
        List<FunctionMetric> FunctionMetricList = new List<FunctionMetric>();
        int NumberOfLines, NumberOfComments, NumberOfChildrens, DepthOfInheritance, WeightedMethods, begin, end;
        string filePath, description, name;
        string[] content;

        public ClassMetric(int begin, int end, string name, string filePath)
        {
            this.begin = begin;
            this.end = end;
            this.name = name;
            this.filePath = filePath;
        }
    }
}
