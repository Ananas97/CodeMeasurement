using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class FunctionMetric : MetricContent
    {
        public int NumberOfLines, NumberOfComments, NestedBlockDepth;

        public FunctionMetric(int begin, int end, string name, string filePath) : base(begin, end, name, filePath) 
        { 
        
        }
    }
}
