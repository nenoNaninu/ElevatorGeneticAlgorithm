﻿using ElevatorGeneticAlgorithm.Model;
using ElevatorGeneticAlgorithm.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevatorGeneticAlgorithm
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var totalPeopleNum = Database.Configuration.TotalPeopleNumber;
            var genericNumber = Database.Configuration.GenericNumber;
            var pairNumber = Database.Configuration.PairNumberOfCrossoverParents;
            var mutationRate = Database.Configuration.MutationRate;
            var maxCarrying = Database.Configuration.MaxLoadingNum;
            var elevatorSpeed = Database.Configuration.ElevatorSpeed;
            var openDoorSpeed = Database.Configuration.OpenDoorTime;

            //遺伝子の配列。
            var generics = new List<Genetic>(genericNumber);

            for (int i = 0; i < totalPeopleNum; i++)
            {
                generics.Add(new Genetic(totalPeopleNum));
            }

            //人の配列
            var peoples = await Database.ReadPeoples();

            //ここから評価関数
            await GeneticAlgorithm.Learning(10, generics, peoples, pairNumber, mutationRate, genericNumber, maxCarrying, elevatorSpeed, openDoorSpeed);

            Console.WriteLine("finish!");
            Console.ReadLine();
        }
    }
}
