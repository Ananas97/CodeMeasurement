using System;
using System.IO;
using System.Text.RegularExpressions;

sealed class NumberOfClasses : Measurement
{
    public NumberOfClasses(string ProjectPath) : base(ProjectPath) {}

    protected override int DoMeasure() {
        int counter = 0; 
        // to be continued...
        string pattern = @"proper regex here";
        Regex rg = new Regex(pattern);

        foreach (string file in Files) {
            var lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                counter += rg.Matches(line).Count; 
            }
        }

        Console.WriteLine("I've counted the number of classes.");
        return counter;
    }
}