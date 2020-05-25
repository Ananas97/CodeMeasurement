using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CodeMeasurement.Measurements.StorageObjects;
using System.Text.RegularExpressions;


namespace CodeMeasurement.Measurements
{
    class CodeMeasure
    {
        public List<String> Files { get; set; }
        public string ProjectPath { get; set; }
        public GeneralMetric generalMetric = new GeneralMetric();
 

        public CodeMeasure(string ProjectPath)
        {
            this.ProjectPath = ProjectPath;
            this.Files = DirSearch(ProjectPath);
        }

        public void GoThroughGeneralMetric()
        {
            foreach(string file in Files)
            {
                string[] fileContent = File.ReadAllLines(file);
                generalMetric.NumberOfLines += CalculateLinesOfCode(fileContent);
                generalMetric.NumberOfComments += CalculateLinesOfComments(fileContent);
                CatchClass(fileContent, file);
            }
        }

        public void GoThroughClassMetric()
        {

        }

        private void CatchClass(string[] fileContent, string filePath)
        {
            Regex rg = new Regex(@" class ", RegexOptions.IgnoreCase);
            int counterStart = 0, counterEnd = 0;

            foreach (string line in fileContent)
            {
                if(rg.IsMatch(line))
                {
                    string name = Regex.Match(line, @"class\S*(?:\s\S+)?", RegexOptions.IgnoreCase).ToString().Substring(6);
                    
                    Console.WriteLine(name);
                    //

                    // here zrobić counterEnd i wtedy można od razu wrzucać content i count lines i count comments
                    // a start i end to będą indeksy fileContent dla danego file

                    ClassMetric classMetric = new ClassMetric(counterStart, 0, name, filePath);


                    generalMetric.ClassMetricList.Add(classMetric);
                    //
                }
                counterStart++;
            }
        }

        private int CalculateLinesOfCode(string[] fileContent)
        {
            return fileContent.Length;
        }

        private int CalculateLinesOfComments(string[] fileContent)
        {
            int counter = 0;

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

            return counter;
        }

        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    if (f.EndsWith(".cs"))
                    {
                        files.Add(f);
                    }
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return files;
        }

    }
}
