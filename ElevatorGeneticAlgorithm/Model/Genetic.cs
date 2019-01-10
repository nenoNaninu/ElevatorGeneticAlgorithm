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
        MinimizeSigmoidWaitingTime,
    }

    public class Genetic : IEnumerable<int>
    {
        private List<int> _geneticList;
        public double EvaluationValue { get; set; } = double.MinValue;

        /// <summary>
        /// 遺伝子の長さ
        /// </summary>
        public int Length => _geneticList.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genetic">遺伝子の中身</param>
        public Genetic(List<int> genetic)
        {
            _geneticList = genetic;
        }

        /// <summary>
        /// シリアライザとデシリアライザのためだけにデフォルトコンストラクタは残してある。
        /// </summary>
        public Genetic()
        {

        }


        public Genetic(Genetic genetic)
        {
            _geneticList = new List<int>(genetic._geneticList);
        }

        /// <summary>
        /// </summary>
        /// <param name="geneticLength">長さ</param>
        public Genetic(int geneticLength)
        {
            _geneticList = RandomList(geneticLength);
        }

        public double Evaluate(EvaluateMethod method, List<Person> people)
        {
            if (method == EvaluateMethod.MinimizeIndividualWaitingTime)
            {
                EvaluationValue = EvaluateIndividualWaitingTime(people);
                return EvaluationValue;
            }
            else if (method == EvaluateMethod.MinimizeOverallWaitTime)
            {
                return EvaluateOverallWaitTime();
            }
            else if (method == EvaluateMethod.MinimizeSigmoidWaitingTime)
            {
                EvaluationValue = EvaluateSigmoidWaitingTime(people);
                return EvaluationValue;
            }
            else
            {
                return -1;
            }
        }


        private double EvaluateIndividualWaitingTime(List<Person> people)
        {
            return people.Sum(x => x.WaitingTime * x.WaitingTime);
        }

        private double EvaluateOverallWaitTime()
        {
            return -1;
        }

        private double EvaluateSigmoidWaitingTime(List<Person> people)
        {
            var t = 3.0;
            return people.Sum(x => 100 / (1 + Math.Exp(x.WaitingTime - t)));
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
            foreach (var g in _geneticList)
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
            get => _geneticList[idx];
            set => _geneticList[idx] = value;
        }
    }
}