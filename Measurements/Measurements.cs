using CodeMeasurement.Measurements.StorageObjects;
using System;
using System.Collections.Generic;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "C:\\Users\\Pawe³\\source\\repos\\CodeMeasurement\\Measurements\\demo";
    public static void main() {

        DatabaseManagement databaseManagement = new DatabaseManagement("Host=localhost;Username=postgres;Password=postgres;Database=postgres");
        List<ProjectInfo> projectInfo = databaseManagement.getEveryProjects("test@gmail.com");
        //Console.WriteLine("MEASUREMENTS");
        //foreach(ProjectInfo project in projectInfo)
        //{
        //    Console.WriteLine(project.name + " " + project.source + " " + project.creationDate);
        //}

        //Console.WriteLine(databaseManagement.loginUser("XD", "XDD"));
        //Console.WriteLine(databaseManagement.loginUser("test@gmail.com", "test123"));

        Console.WriteLine(databaseManagement.saveProject("ucze sie java2", "test@gmail.com", "local source"));

        Console.ReadLine();
        //CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);

        //foreach(string file in codeMeasure.Files)
        //{
        //    Console.WriteLine(file);
        //}

        //codeMeasure.GoThroughGeneralMetric();

        //Console.WriteLine("Classes: ");
        //foreach (var clazz in codeMeasure.generalMetric.ClassMetricList)
        //{
        //    Console.WriteLine(clazz.begin + " " + clazz.end + " " + clazz.name);
        //    //Console.WriteLine("content:");
        //    //clazz.PrintContent();
        //    Console.WriteLine("\tFunctions: ");
        //    foreach (var func in clazz.FunctionMetricList)
        //    {
        //        Console.WriteLine("\t" + func.begin + " " + func.end + " " + func.name);
        //        Console.WriteLine("\tcontent:");
        //        func.PrintContent();
        //    }
        //}


        //Console.WriteLine("Code: " + codeMeasure.generalMetric.NumberOfLines);
        //Console.WriteLine("Comments: " + codeMeasure.generalMetric.NumberOfComments);

        //Console.WriteLine("Let's measure this project's code!");
        //Console.WriteLine();
        
        //Measurement LoC = new LinesOfCode(ProjectPath);
        //Measurement NoC = new NumberOfClasses(ProjectPath);
        //Measurement NoComments = new LinesOfComments(ProjectPath);

        //Console.WriteLine();
        //Console.WriteLine("RESULTS");
        //Console.WriteLine("LOC: " + LoC.Result);
        //Console.WriteLine("NOC: " + NoC.Result);
        //Console.WriteLine("NoComments: " + NoComments.Result);
    }

}
