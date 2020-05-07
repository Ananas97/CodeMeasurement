using System;
using System.IO;
abstract class Measurement
{
    protected string[] Files { get; set; }
    protected string ProjectPath { get; set; }
    public int Result { get; protected set; }

    public Measurement(string ProjectPath) {
        Console.WriteLine("Measurement created");
        this.ProjectPath = ProjectPath;
        this.Files       = Directory.GetFiles(ProjectPath);
        this.Result      = DoMeasure();
    }
    protected abstract int DoMeasure();

}
