using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CodeMeasurement.Measurements.StorageObjects;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Xml;
using System.Linq;

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

        public void Calculate() 
        {
            GoThroughGeneralMetric();
            GoThroughClassMetric();
            GoThroughFunctionMetric();
        }

        private void GoThroughGeneralMetric()
        {
            foreach(string file in Files)
            {
                string[] fileContent = File.ReadAllLines(file);
                generalMetric.NumberOfLines += CalculateLinesOfCode(fileContent);
                generalMetric.NumberOfComments += CalculateLinesOfComments(fileContent);
                CatchClasses(fileContent, file);               
            }
            generalMetric.NumberOfNamespaces = CalculateNumberOfNamespaces();
            generalMetric.NumberOfClasses = generalMetric.ClassMetricList.Count;
        }

        private void GoThroughClassMetric()
        {
            foreach (ClassMetric classMetric in generalMetric.ClassMetricList)
            {
                string[] classContent = classMetric.GetContent().ToArray();

                classMetric.NumberOfLines = CalculateLinesOfCode(classContent);
                classMetric.NumberOfComments = CalculateLinesOfComments(classContent);

                classMetric.DepthOfInheritance = CalculateDepthOfInheritence(classMetric);
                classMetric.NumberOfChildrens = CalculateNumberOfChildren(classMetric);
                classMetric.WeightedMethods = CalculateWeightedMethods(classMetric);

                CatchFunctions(classMetric, classMetric.FilePath);
            }
        }

        private void GoThroughFunctionMetric()
        {
            foreach (ClassMetric classMetric in generalMetric.ClassMetricList)
            {
                foreach (FunctionMetric functionMetric in classMetric.FunctionMetricList)
                {
                    string[] functionContent = functionMetric.GetContent().ToArray();

                    functionMetric.NumberOfLines = CalculateLinesOfCode(functionContent);
                    functionMetric.NumberOfComments = CalculateLinesOfComments(functionContent);

                    functionMetric.NestedBlockDepth = CalculateNestedBlockDepth(functionMetric);
                }
            }
        }

        private void CatchClasses(string[] fileContent, string filePath)
        {
            Regex rg = new Regex(@" class ", RegexOptions.IgnoreCase);
            Regex rgStartingBrace = new Regex("{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex("}", RegexOptions.IgnoreCase);

            Boolean inClass = false;
            int braceCounter = 0;
            string name = "default";
            string nameOfSuperClass = "none";
            int lineCounter = 1, start = 0;
            //ClassMetric currentClassMetric = new ClassMetric(0, 0, "", "");

            foreach (string line in fileContent)
            {
                if (!inClass)
                {
                    // look for a begning of a class
                    if (rg.IsMatch(line))
                    {
                        // Console.WriteLine("I've found a class!");
                        // save starting line
                        start = lineCounter;
                        // save name of the class 
                        // name = Regex.Match(line, @"class\S*(?:\s\S+)?", RegexOptions.IgnoreCase).ToString().Substring(6);
                        GroupCollection groups = Regex.Match(line, @"class\s+([\S\d]+)\s*(?::\s*([\S\d]+))?", RegexOptions.IgnoreCase).Groups;
                        name = groups[1].Value;
                        if (!groups[2].Value.Equals(String.Empty)) 
                        {
                            nameOfSuperClass = groups[2].Value;
                        }
                        Console.WriteLine("pryt" + nameOfSuperClass);
                        inClass = true;
                    }
                }
                else // we are in the class body
                {
                    // wait for the 1st brace
                    if (braceCounter == 0)
                    {
                        int c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter += c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) 
                        { 
                            braceCounter -= c;
                            // check if we made it to the end of class
                            if (braceCounter <= 0)
                            {
                                AddClassMetric(start, lineCounter, name, nameOfSuperClass, filePath);
                                inClass = false;
                                braceCounter = 0;
                            }
                        }
                    }
                    else 
                    {
                        int c;
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter += c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter -= c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }
                        
                        if (braceCounter <= 0) // we made it to the end of the class!
                        {
                            AddClassMetric(start, lineCounter, name, nameOfSuperClass, filePath);
                            inClass = false;
                            braceCounter = 0;
                        }
                    }
                    //currentClassMetric.content.Add(line);
                }
                lineCounter++;
            }
        }
        
        private void CatchFunctions(ClassMetric classMetric, string filePath)
        {
            //Regex rg = new Regex(@"\(.*\)\s*\n*{", RegexOptions.IgnoreCase);
            Regex rg = new Regex(@"\s+([\S\d]+)\s*\(.*\)\s*[^;]", RegexOptions.IgnoreCase);
            
            Regex rgStartingBrace = new Regex("{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex("}", RegexOptions.IgnoreCase);

            Boolean inFunction = false;
            string name = "default";
            int braceCounter = 0, lineCounter = classMetric.Begin, begin = 0;

            foreach (string line in classMetric.GetContent())
            {
                if (!inFunction)
                {
                    // look for a begning of a class
                    if (rg.IsMatch(line))
                    {
                        // Console.WriteLine("I've found a function!");
                        // save starting line
                        begin = lineCounter;
                        // save name of the class 
                        //name = Regex.Match(line, @"\s+([\S\d]+)\s*\(.*\)\s*\n*\s*{", RegexOptions.IgnoreCase).Groups[1].Value;
                        name = Regex.Match(line, @"\s+([\S\d]+)\s*\(.*\)\s*[^;]", RegexOptions.IgnoreCase).Groups[1].Value;
                        inFunction = true;

                    }
                }
                else // we are in the function body
                {
                    // wait for the 1st brace
                    if (braceCounter == 0)
                    {
                        int c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter += c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0)
                        {
                            braceCounter -= c;
                            // check if we made it to the end of the function
                            if (braceCounter <= 0)
                            {
                                AddFunctionMetric(begin, lineCounter, name, filePath, classMetric);
                                inFunction = false;
                                braceCounter = 0;
                            }
                        }
                    }
                    else
                    {
                        int c;
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter += c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) { 
                            braceCounter -= c; 
                            //Console.WriteLine(braceCounter + " " + line); 
                        }

                        if (braceCounter <= 0) // we made it to the end of the function!
                        {
                            AddFunctionMetric(begin, lineCounter, name, filePath, classMetric);
                            inFunction = false;
                            braceCounter = 0;
                        }
                    }
                }
                lineCounter++;
            }
        }
        
        private void OldCatchFunction(ClassMetric classMetric, String file)
        {
            // regex for function in line, avoid key-words 
            Regex rg = new Regex(@"\(.*\)\s*\n*{", RegexOptions.IgnoreCase);
            Regex rgStartingBrace = new Regex("{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex("}", RegexOptions.IgnoreCase);
            // Regex rgStartingBrace = new Regex("{(?=[^\"]*(?:\"[^\"]*\"[^\"]*)*$)", RegexOptions.IgnoreCase);
            // Regex rgClosingBrace = new Regex("}(?=[^\"]*(?:\"[^\"]*\"[^\"]*)*$)", RegexOptions.IgnoreCase);

            Boolean in_function = false;
            int braceCounter = 0;
            string name;
            int counter = classMetric.Begin, start;
            FunctionMetric currentFunctionMetric = new FunctionMetric(0, 0, "", "");

            Console.WriteLine("Another class");

            foreach (string line in classMetric.GetContent())
            {
                Console.WriteLine("Current line: " + line);
                if (!in_function)
                {
                    if (rg.IsMatch(line))
                    {
                        // Console.WriteLine("I've found a function!");
                        start = counter;
                        // this will hopefully return the name of the function
                        name = Regex.Match(line, @"(.*)\s*\(.*\)\s*\n*{", RegexOptions.IgnoreCase).Groups[1].Value;
                        Console.WriteLine("name: " + name);

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
                    if (braceCounter == 0)
                    {
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { braceCounter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) 
                        {
                            braceCounter -= c;
                            if (braceCounter <= 0) 
                            {
                                currentFunctionMetric.End = counter;
                                in_function = false;
                            }
                        }
                    }
                    else
                    {
                        c = rgStartingBrace.Matches(line).Count;
                        if (c > 0) { braceCounter += c; }
                        c = rgClosingBrace.Matches(line).Count;
                        if (c > 0) { braceCounter -= c; }

                        if (braceCounter <= 0) // we made it to the end of the function!
                        {
                            currentFunctionMetric.End = counter;
                            in_function = false;
                        }
                    }
                    //currentFunctionMetric.content.Add(line);                   
                }
                counter++;
            }
        }

        private int CalculateLinesOfCode(string[] content)
        {
            return content.Length;
        }

        private int CalculateLinesOfComments(string[] content)
        {
            int counter = 0;

            foreach (string line in content)
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

        private int CalculateNumberOfNamespaces()
        {
            List<string> namespaces = new List<string>();
            Regex rg = new Regex(@"^namespace\s+", RegexOptions.IgnoreCase);
            foreach (string file in Files)
            {
                string[] fileContent = File.ReadAllLines(file);
                foreach (string line in fileContent)
                {
                    if (rg.IsMatch(line)) 
                    {
                        // Console.WriteLine("I've found a ns!");
                        string names = Regex.Match(line, @"^namespace\s+([\S\d]+)(?:.[\S\d])*", RegexOptions.IgnoreCase).Groups[1].Value;
                        foreach (string ns in names.Split("."))
                        {
                            namespaces.Add(ns);
                        }                      
                    }
                }
            }  
            return namespaces.Distinct().ToList().Count;
        }

        private int CalculateDepthOfInheritence(ClassMetric classMetric) 
        {
            return 1;
        }

        private int CalculateNumberOfChildren(ClassMetric classMetric)
        {
            return 1;
        }

        private int CalculateWeightedMethods(ClassMetric classMetric)
        {
            return 1;
        }

        private int CalculateNestedBlockDepth(FunctionMetric functionMetric)
        {
            Regex rgStartingBrace = new Regex("{", RegexOptions.IgnoreCase);
            Regex rgClosingBrace = new Regex("}", RegexOptions.IgnoreCase);
            int max = 0;
            int braceCounter = 0;
            foreach (string line in functionMetric.GetContent()) 
            {
                braceCounter += rgStartingBrace.Matches(line).Count;
                if (braceCounter > max) 
                {
                    max = braceCounter;
                }
                braceCounter -= rgClosingBrace.Matches(line).Count;
            }
            return max;
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

        private void AddClassMetric(int begin, int end, string name, string nameOfSuperClass, string filePath) {
            var classMetric = new ClassMetric(begin, end, name, nameOfSuperClass, filePath);
            generalMetric.ClassMetricList.Add(classMetric);
        }

        private void AddFunctionMetric(int begin, int end, string name, string filePath, ClassMetric classMetric)
        {
            var functionMetric = new FunctionMetric(begin, end, name, filePath);
            classMetric.FunctionMetricList.Add(functionMetric);
        }

        public void PrintMetrics() 
        {
            PrintGeneralMetrics();
            foreach (ClassMetric classMetric in generalMetric.ClassMetricList)
            {
                PrintClassMetrics(classMetric);
                foreach (FunctionMetric functionMetric in classMetric.FunctionMetricList)
                {
                    PrintFunctionMetrics(functionMetric);
                }
            }
        }

        private void PrintGeneralMetrics()
        {
            Console.WriteLine("----- GENERAL METRICS -----");
            Console.WriteLine("NumberOfLines: \t\t" + generalMetric.NumberOfLines);
            Console.WriteLine("NumberOfComments: \t" + generalMetric.NumberOfComments);
            Console.WriteLine("NumberOfNamespaces: \t" + generalMetric.NumberOfNamespaces);
            Console.WriteLine("NumberOfClasses: \t" + generalMetric.NumberOfClasses);
        }

        private void PrintClassMetrics(ClassMetric classMetric)
        {
            WriteLineClass("----- CLASS METRICS -----");
            WriteLineClass("NAME: \t\t\t" + classMetric.Name);
            if (!classMetric.NameOfSuperClass.Equals("none"))
            {
                WriteLineClass("NameOfSuperClass \t" + classMetric.NameOfSuperClass);
            }
            WriteLineClass("NumberOfLines \t\t" + classMetric.NumberOfLines);
            WriteLineClass("NumberOfComments \t" + classMetric.NumberOfComments);
            WriteLineClass("NumberOfChildrens \t" + classMetric.NumberOfChildrens);
            WriteLineClass("DepthOfInheritance \t" + classMetric.DepthOfInheritance);
            WriteLineClass("WeightedMethods \t" + classMetric.WeightedMethods);
        }

        private void PrintFunctionMetrics(FunctionMetric functionMetric)
        {
            WriteLineFunction("----- FUNCTION METRICS -----");
            WriteLineFunction("NAME: \t\t\t" + functionMetric.Name);
            WriteLineFunction("NumberOfLines \t\t" + functionMetric.NumberOfLines);
            WriteLineFunction("NumberOfComments \t" + functionMetric.NumberOfComments);
            WriteLineFunction("NestedBlockDepth \t" + functionMetric.NestedBlockDepth);
        }

        private void WriteLineClass(string print)
        {
            Console.WriteLine("\t" + print);
        }

        private void WriteLineFunction(string print)
        {
            Console.WriteLine("\t\t" + print);
        }
    }
}
