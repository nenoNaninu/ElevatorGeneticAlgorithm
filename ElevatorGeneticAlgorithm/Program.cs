using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ElevatorGeneticAlgorithm.Model;
using ElevatorGeneticAlgorithm.Repository;

namespace ElevatorGeneticAlgorithm
{
    class Program
    {


        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var totalPeopleNum = Database.Configuration.TotalPeopleNumber;
                var genericNumber = Database.Configuration.GenericNumber;

                //遺伝子の配列。
                var generics = new List<Genetic>(genericNumber);

                for (int i = 0; i < totalPeopleNum; i++)
                {
                    generics.Add(new Genetic(totalPeopleNum));
                }

                //人の配列
                var peoples = await Database.ReadPeoples();

                ////最大積載人数

                ////ここから評価関数
                GeneticAlgorithm.Learning(1, generics, peoples);

                Console.WriteLine($"{DateTime.Now:yyyyMMddHHmmss}");
                Console.ReadLine();

            }).Wait();
        }
    }
}
