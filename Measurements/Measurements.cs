using System;
using System.IO;

static class MeasurementsDemo
{
    private static string ProjectPath = Directory.GetCurrentDirectory(); 
    public static void main() {

        Console.WriteLine("Let's measure this project's code!");
        Console.WriteLine();

        Measurement LoC = new LinesOfCode(ProjectPath);
        Measurement NoC = new NumberOfClasses(ProjectPath);

        Console.WriteLine();
        Console.WriteLine("RESULTS");
        Console.WriteLine("LOC: " + LoC.Result);
        Console.WriteLine("NOC: " + NoC.Result);
    }

}
