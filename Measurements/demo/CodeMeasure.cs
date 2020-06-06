using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CodeMeasurement.Measurements.StorageObjects;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace CodeMeasurement.Measurements
{
    class CodeMeasur
    {
        public List<String> Files { get; set; }
        public string ProjectPath { get; set; }
        public GeneralMetric generalMetric = new GeneralMetric();
 

        public CodeMeasur(string ProjectPath)
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
                foreach (ClassMetric classMetric in generalMetric.ClassMetricList) {
                    //CatchFunction(classMetric, file);
                    classMetric.ShowContent();
                    Console.WriteLine(); 
                }
            }
        }

        public void GoThroughClassMetric()
        {
         
        }

        private void CatchClass(string[] fileContent, string filePath)
        {
            Regex rg = new Regex(@" class ", RegexOptions.IgnoreCase);
            Regex rgStartingBrace = new Regex(@"{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex(@"}", RegexOptions.IgnoreCase);

            Boolean in_class = false;
            int brace_counter = 0;
            string name = "default";
            int counter = 0, start;
            ClassMetric currentClassMetric = new ClassMetric(0, 0, "", "");

            foreach (string line in fileContent)
            {
                if (!in_class)
                {
                    if (rg.IsMatch(line))
                    {
                        start = counter;
                        name = Regex.Match(line, @"class\S*(?:\s\S+)?", RegexOptions.IgnoreCase).ToString().Substring(6);
                        Console.WriteLine(name);
                        in_class = true;
                        currentClassMetric = new ClassMetric(start, counter, name, filePath);
                        generalMetric.ClassMetricList.Add(currentClassMetric);
                    }
                }
                else // we are in the class body
                {
                    // wait for the 1st brace
                    if (brace_counter == 0)
                    {
                        int c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) 
                        { 
                            brace_counter -= c;
                            if (brace_counter <= 0)
                            {
                                currentClassMetric.end = counter;
                                in_class = false;
                            }
                        }
                    }
                    else 
                    {
                        int c;
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter -= c; }
                        
                        if (brace_counter <= 0) // we made it to the end of the class!
                        {
                            currentClassMetric.end = counter;
                            in_class = false;
                        }
                    }
                    currentClassMetric.content.Add(line);
                }
                counter++;
            }
        }

        private void CatchFunction(ClassMetric classMetric, String file)
        {
            // regex for function in line, avoid key-words 
            Regex rg = new Regex(@"\(.*\)\s*\n*{", RegexOptions.IgnoreCase);
            Regex rgStartingBrace = new Regex(@"{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex(@"}", RegexOptions.IgnoreCase);

            Boolean in_function = false;
            int brace_counter = 0;
            string name = "";
            int counter = 0, start = 0;
            FunctionMetric currentFunctionMetric = new FunctionMetric(0, 0, "", "");

            Console.WriteLine("Another class");

            foreach (string line in classMetric.content)
            {
                Console.WriteLine("Current line: " + line);
                if (!in_function)
                {
                    if (rg.IsMatch(line))
                    {
                        Console.WriteLine("I've found a function!");
                        start = counter;
                        // this will hopefully return the name of the function
                        name = Regex.Match(line, @"\s*<func>\(.*\)\s*\n*{", RegexOptions.IgnoreCase).Groups["func"].Value;
                        Console.WriteLine(name);

                        in_function = true;

                        currentFunctionMetric = new FunctionMetric(start, counter, name, file);
                        classMetric.FunctionMetricList.Add(currentFunctionMetric);
                    }
                }
                else // we are in the function body
                {
                    Console.WriteLine("inside");
                    // wait for the 1st brace
                    int c;
                    if (brace_counter == 0)
                    {
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) 
                        {
                            brace_counter -= c;
                            if (brace_counter <= 0) 
                            {
                                currentFunctionMetric.end = counter;
                                in_function = false;
                            }
                        }
                    }
                    else
                    {
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) { brace_counter -= c; }

                        if (brace_counter <= 0) // we made it to the end of the function!
                        {
                            currentFunctionMetric.end = counter;
                            in_function = false;
                        }
                    }
                    currentFunctionMetric.content.Add(line);
                }
                counter++;
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
