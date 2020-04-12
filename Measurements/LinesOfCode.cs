using System;

sealed class LinesOfCode : Measurement
{
    public LinesOfCode(string ProjectPath) : base(ProjectPath) {
        Console.WriteLine("path: " + ProjectPath);
    }

    protected override int TakeMeasurement() {
        Console.WriteLine("I've counted the lines of code.");
        return 20;
    }
}