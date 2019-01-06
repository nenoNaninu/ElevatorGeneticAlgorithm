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

            string targetFilePath = Path.Combine(appDataPath, "ElevatorGA/peoples.json");

            if (File.Exists(targetFilePath))
            {
                using (var sr = new StreamReader(targetFilePath, Encoding.UTF8))
                {
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
                await JsonSerializer.SerializeAsync(sw.BaseStream, _peoples);
            }

            return _peoples;
        }
    }



}
}