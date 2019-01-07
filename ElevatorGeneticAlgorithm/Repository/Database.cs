using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ElevatorGeneticAlgorithm.Model;
using Utf8Json;

namespace ElevatorGeneticAlgorithm.Repository
{
    public static class Database
    {
        private static List<Person> _peoples = null;
        private static Configuration _configuration = null;
        private static string _targetDir = null;

        private static string TargetDir
        {
            get
            {
                if (_targetDir != null) return _targetDir;

                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var targetDir = Path.Combine(appDataPath, "ElevatorGA", $"{DateTime.Now:yyyyMMddHHmmss}");

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                _targetDir = targetDir;

                return _targetDir;
            }
        }
        public static Configuration Configuration => _configuration ?? (_configuration = ReadConfigurationFromFile());

        /// <summary>
        /// まぁ後回し。とりあえず規定値で。
        /// </summary>
        /// <returns></returns>
        public static Configuration ReadConfigurationFromFile()
        {
            return new Configuration();
        }

        public static async Task<List<Person>> ReadPeoples()
        {

            if (_peoples != null)
            {
                return _peoples;
            }

            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var targetDir = Path.Combine(appDataPath, "ElevatorGA");

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            string targetFilePath = Path.Combine(appDataPath, "ElevatorGA/peoples.json");

            if (File.Exists(targetFilePath))
            {
                using (var sr = new StreamReader(targetFilePath, Encoding.UTF8))
                {
                    Console.WriteLine($"read people data from {targetFilePath}");
                    return _peoples = await JsonSerializer.DeserializeAsync<List<Person>>(sr.BaseStream);
                }
            }
            else
            {
                //ランダムに生成する。
                int totalPeople = Configuration.TotalPeopleNumber;
                double span = Configuration.SamplingTimeSpan;

                _peoples = new List<Person>(totalPeople);
                var rand = new Random();

                for (int i = 0; i < totalPeople; i++)
                {
                    var targetFloor = rand.Next(1, 20);
                    var currentFloor = rand.Next(1, 20);

                    while (targetFloor == currentFloor)
                    {
                        targetFloor = rand.Next(1, 20);
                    }

                    _peoples.Add(new Person
                    {
                        Id = i,
                        TargetFloor = targetFloor,
                        CurrentFloor = currentFloor,
                        StartWaitingTime = rand.NextDouble() * span,
                    });
                }

                //それは保存しておく。
                using (var sw = new StreamWriter(targetFilePath, false, Encoding.UTF8))
                {
                    Console.WriteLine($"save people data from {targetFilePath}");
                    await JsonSerializer.SerializeAsync(sw.BaseStream, _peoples);
                }

                return _peoples;
            }
        }

        public static async Task SaveGenetic(List<Genetic> genetics, int iteration)
        {
            var targetFilePath = Path.Combine(TargetDir, $"Genetics{iteration,000}.json");

            //それは保存しておく。
            using (var sw = new StreamWriter(targetFilePath, false, Encoding.UTF8))
            {
                await JsonSerializer.SerializeAsync(sw.BaseStream, genetics, Utf8Json.Resolvers.StandardResolver.AllowPrivate);
            }
        }


    }
}