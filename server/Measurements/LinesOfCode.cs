using System;
using System.IO;

sealed class LinesOfCode : Measurement
{
    public LinesOfCode(string ProjectPath) : base(ProjectPath) {}

    protected override int DoMeasure() {
        int counter = 0; 
        foreach (string file in Files) {
            int lines = File.ReadAllLines(file).Length;
            counter += lines;
        }

        Console.WriteLine("I've counted the lines of code.");
        return counter;
    }
}