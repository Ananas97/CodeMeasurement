using System;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "C:\\Users\\Pawe³\\source\\repos\\CodeMeasurement\\Measurements\\demo";
    public static void main() {

        CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);
        codeMeasure.Calculate();

        codeMeasure.PrintMetrics();
    }

}
