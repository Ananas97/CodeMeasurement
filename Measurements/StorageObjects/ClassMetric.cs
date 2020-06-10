using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class ClassMetric : MetricContent
    {
        public List<FunctionMetric> FunctionMetricList = new List<FunctionMetric>();
        public int NumberOfLines, NumberOfComments, NumberOfChildrens, DepthOfInheritance, WeightedMethods, classMetricId;
        public string NameOfSuperClass;

        public ClassMetric(int begin, int end, string name, string nameOfSuperClass, string filePath) : base(begin, end, name, filePath)
        { 
            Console.WriteLine(nameOfSuperClass);
            this.NameOfSuperClass = nameOfSuperClass;
        }
    }
}
