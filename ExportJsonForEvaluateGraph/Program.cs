using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ElevatorGeneticAlgorithm.Model;
using Utf8Json;

namespace ExportJsonForEvaluateGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            var evaluate = new List<double>();

            var path = args[0];
            Console.WriteLine($"Read from {path}");

            var files = Directory.GetFiles(path).OrderBy(f => f);

            foreach (var file in files)
            {
                Console.WriteLine(file);
                using (var sr = new StreamReader(file, Encoding.UTF8))
                {
                    var genetics = JsonSerializer.Deserialize<List<Genetic>>(sr.BaseStream);
                    var mean = genetics.Average(x => x.EvaluationValue);
                    evaluate.Add(mean);
                }
            }

            var outputPath = Path.Combine(path, "evalu.json");
            using (var sw = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                Console.WriteLine($"save people data from {outputPath}");
                JsonSerializer.Serialize(sw.BaseStream, evaluate);
            }

            Console.ReadLine();
        }
    }
}
