using CodeMeasurement.Measurements.StorageObjects;
using System;
using System.Collections.Generic;
using System.IO;

static class MeasurementsDemo
{
    //private static string ProjectPath = Directory.GetCurrentDirectory(); 
    private static string ProjectPath = "C:\\Users\\Pawe³\\source\\repos\\CodeMeasurement\\Measurements\\demo";
    public static void main() {

        CodeMeasurement.Measurements.CodeMeasure codeMeasure = new CodeMeasurement.Measurements.CodeMeasure(ProjectPath);

        DatabaseManagement databaseManagement = new DatabaseManagement("Host=localhost;Username=postgres;Password=postgres;Database=postgres");

        List<ProjectInfo> projectInfoList = new List<ProjectInfo>();
        ProjectInfo projectInfo = new ProjectInfo(1, 1, DateTime.Now, DateTime.Now, "test@gmail.com", "test project");

        List<GeneralMetric> generalMetrics = new List<GeneralMetric>();

        GeneralMetric generalMetric1 = new GeneralMetric();
        generalMetric1.NumberOfClasses = 12;
        generalMetric1.NumberOfComments = 13;
        generalMetric1.NumberOfLines = 14;
        generalMetric1.NumberOfNamespaces = 15;
        generalMetric1.updateDate = DateTime.Now.ToString();

        List<ClassMetric> classMetrics = new List<ClassMetric>();
        ClassMetric classMetric1 = new ClassMetric(0, 0, "test class", "", "");
        classMetric1.NumberOfChildrens = 112;
        classMetric1.NumberOfComments = 113;
        classMetric1.NumberOfLines = 114;
        classMetric1.WeightedMethods = 115;
        classMetric1.DepthOfInheritance = 116;

        List<FunctionMetric> functionMetrics1 = new List<FunctionMetric>();
        FunctionMetric fmetric = new FunctionMetric(0, 0, "test function 1", "");
        fmetric.NestedBlockDepth = 3;
        fmetric.NumberOfComments = 4;
        fmetric.NumberOfLines = 5;
        functionMetrics1.Add(fmetric);
        classMetric1.FunctionMetricList = functionMetrics1;

        ClassMetric classMetric2 = new ClassMetric(0, 0, "test class - 2", "", "");
        classMetric2.NumberOfChildrens = 122;
        classMetric2.NumberOfComments = 123;
        classMetric2.NumberOfLines = 124;
        classMetric2.WeightedMethods = 125;
        classMetric2.DepthOfInheritance = 126;

        List<FunctionMetric> functionMetrics2 = new List<FunctionMetric>();
        FunctionMetric fmetric2 = new FunctionMetric(0, 0, "test function 1", "");
        fmetric2.NestedBlockDepth = 333;
        fmetric2.NumberOfComments = 444;
        fmetric2.NumberOfLines = 555;
        functionMetrics2.Add(fmetric2);
        classMetric2.FunctionMetricList = functionMetrics2;

        classMetrics.Add(classMetric1);
        classMetrics.Add(classMetric2);
    
        generalMetric1.ClassMetricList = classMetrics;
        generalMetrics.Add(generalMetric1);
        projectInfo.generalMetricList = generalMetrics;

        // SEND TO FRONT projectInfo

        //codeMeasure.Calculate();

        //codeMeasure.PrintMetrics();
    }

}
