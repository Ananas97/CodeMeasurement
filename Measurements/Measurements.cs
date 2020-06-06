using System;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "C:\\Users\\Pawe�\\source\\repos\\CodeMeasurement\\Measurements\\demo";
    public static void main() {

        CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);

        foreach(string file in codeMeasure.Files)
        {
            Console.WriteLine(file);
        }

        codeMeasure.GoThroughGeneralMetric();

        Console.WriteLine("Classes: ");
        foreach (var clazz in codeMeasure.generalMetric.ClassMetricList)
        {
            Console.WriteLine(clazz.begin + " " + clazz.end + " " + clazz.name);
            foreach (var func in clazz.FunctionMetricList)
            {
                Console.WriteLine("\t" + func.begin + " " + func.end + " " + func.name + " " + func.content[0]);
            }
        }


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
