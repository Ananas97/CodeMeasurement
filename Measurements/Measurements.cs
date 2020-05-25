using System;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "G:\\Studia\\SE1\\TSD\\CodeMeasurement\\Measurements";
    public static void main() {

        CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);

        foreach(string file in codeMeasure.Files)
        {
            Console.WriteLine(file);
        }

        codeMeasure.GoThroughGeneralMetric();

        Console.WriteLine("Code: " + codeMeasure.generalMetric.NumberOfLines);
        Console.WriteLine("Comments: " + codeMeasure.generalMetric.NumberOfComments);

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
