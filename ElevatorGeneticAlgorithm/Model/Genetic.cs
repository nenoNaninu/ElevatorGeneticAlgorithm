using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorGeneticAlgorithm.Model
{
    public enum EvaluateMethod
    {
        MinimizeIndividualWaitingTime,
        MinimizeOverallWaitTime,
    }

    public class Genetic : IEnumerable<int>
    {
        private List<int> GeneticList { get; set; }
        public int Count => GeneticList.Count;

        public Genetic(List<int> genetic)
        {
            GeneticList = genetic;
        }

        /// <summary>
        /// </summary>
        /// <param name="geneticLength">長さ</param>
        public Genetic(int geneticLength)
        {
            GeneticList = RandomList(geneticLength);
        }


        public double Evaluate(EvaluateMethod method)
        {
            if (method == EvaluateMethod.MinimizeIndividualWaitingTime)
            {
                return EvaluateIndividualWaitingTime();
            }
            else if (method == EvaluateMethod.MinimizeOverallWaitTime)
            {
                return EvaluateOverallWaitTime();
            }
            else
            {
                return -1;
            }
        }


        private double EvaluateIndividualWaitingTime()
        {

        }

        private double EvaluateOverallWaitTime()
        {

        }

        private List<int> RandomList(int num)
        {
            var list = new List<int>(num);
            for (int i = 0; i < num; i++)
            {
                list.Add(i);
            }
            return list.OrderBy(_ => Guid.NewGuid()).ToList();
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (var g in GeneticList)
            {
                yield return g;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int this[int idx]
        {
            get => GeneticList[idx];
            set => GeneticList[idx] = value;
        }
    }
}