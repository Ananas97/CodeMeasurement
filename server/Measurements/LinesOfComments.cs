using System;
using System.IO;
using System.Text.RegularExpressions;

sealed class LinesOfComments : Measurement
{
    public LinesOfComments(string ProjectPath) : base(ProjectPath) { }
    protected override int DoMeasure()
    {
        int counter = 0;
        foreach (string fileName in Files)
        {
            string[] fileContent = File.ReadAllLines(fileName);
            foreach (string line in fileContent)
            {
                string lineExperiment = line.Trim();
                if (lineExperiment.StartsWith("//") ||
                    lineExperiment.StartsWith("/*") ||
                    lineExperiment.StartsWith("*") ||
                    lineExperiment.EndsWith("*/"))
                {
                    counter++;
                }
            }
        }
        return counter;
    }
}