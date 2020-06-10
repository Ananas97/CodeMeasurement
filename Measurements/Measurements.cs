using CodeMeasurement.Measurements.StorageObjects;
using System;
using System.Collections.Generic;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "C:\\Users\\Pawe³\\source\\repos\\CodeMeasurement\\Measurements\\demo";
    public static void main() {

        CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);

        DatabaseManagement databaseManagement = new DatabaseManagement("Host=localhost;Username=postgres;Password=postgres;Database=postgres");
        //codeMeasure.Calculate();

        //codeMeasure.PrintMetrics();
    }

}
