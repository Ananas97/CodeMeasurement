using System;
using System.IO;
using System.Text.RegularExpressions;

sealed class NumberOfClasses : Measurement
{
    public NumberOfClasses(string ProjectPath) : base(ProjectPath) {}
    protected override int DoMeasure() {
        int counter = 0; 
        Regex rg = new Regex(@" class ", RegexOptions.IgnoreCase);

        foreach (string file in Files) {
            Console.WriteLine(file);
            var lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                if (rg.IsMatch(line))
                {
                    counter++;
                }
            }
        }

        return counter;
    }
}