using System;
using CrosswordSolverEngine;

namespace CrosswordSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new CrosswordSolverEngine.CrosswordSolverEngine(@"C:\Users\way2t\Downloads\OneDrive_1_3-5-2019\english\english.csv");
            Console.WriteLine(string.Join("\r\n", engine.WordSeek("SI________US")));
        }
    }
}
