using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class GeneralMetric
    {
        public List<ClassMetric> ClassMetricList = new List<ClassMetric>();
        public int NumberOfLines, NumberOfComments, NumberOfNamespaces, NumberOfClasses;
        
    }
}
