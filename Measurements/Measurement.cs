using System;

abstract class Measurement
{
    protected string ProjectPath { get; set; }
    public int Result { get; protected set; }

    public Measurement (string ProjectPath) {
        Console.WriteLine("Measurement created");
        this.ProjectPath = ProjectPath;
        this.Result = TakeMeasurement();
    }
    protected abstract int TakeMeasurement();

}
