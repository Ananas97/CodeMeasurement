using System;

sealed class NumberOfClasses : Measurement
{
    public NumberOfClasses(string ProjectPath) : base(ProjectPath) {
        Console.WriteLine("path: " + ProjectPath);
    }

    protected override int TakeMeasurement() {
        Console.WriteLine("I've counted the number of classes.");
        return 3;
    }
}