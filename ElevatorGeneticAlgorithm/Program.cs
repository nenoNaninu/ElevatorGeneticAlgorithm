using ElevatorGeneticAlgorithm.Model;
using ElevatorGeneticAlgorithm.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                var pairNumber = Database.Configuration.PairNumberOfCrossoverParents;
                var mutationRate = Database.Configuration.MutationRate;
                
                //遺伝子の配列。
                var generics = new List<Genetic>(genericNumber);

                for (int i = 0; i < totalPeopleNum; i++)
                {
                    generics.Add(new Genetic(totalPeopleNum));
                }

                //人の配列
                var peoples = await Database.ReadPeoples();

                //ここから評価関数
                await GeneticAlgorithm.Learning(1000, generics, peoples, pairNumber, mutationRate, genericNumber);

                Console.WriteLine("finish!");
                Console.ReadLine();

            }).Wait();
        }
    }
}
