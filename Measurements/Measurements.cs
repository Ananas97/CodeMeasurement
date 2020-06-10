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

        List<ProjectInfo> projectInfos =  databaseManagement.getEveryProjects("test@gmail.com");
        foreach(ProjectInfo info in projectInfos)
        {
            Console.WriteLine(info.email + " ... " + info.name);
            Console.WriteLine("----");

            foreach (GeneralMetric generalMetric in info.generalMetricList)
            {
                Console.WriteLine(generalMetric.NumberOfClasses.ToString() + " ... " + generalMetric.updateDate);
            }

        }

        //codeMeasure.Calculate();

        //codeMeasure.PrintMetrics();
    }

}
