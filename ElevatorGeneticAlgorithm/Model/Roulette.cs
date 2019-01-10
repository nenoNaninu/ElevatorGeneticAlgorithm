using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Xml.XPath;

namespace ElevatorGeneticAlgorithm.Model
{
    /// <summary>
    /// 評価に応じた確率で回るルーレット。
    /// </summary>
    public class Roulette
    {
        private readonly List<double> _lookupTable;
        private readonly double _sum;
        private readonly Random _random;

        public Roulette(List<Genetic> genetics)
        {
            _random = new Random();
            _lookupTable = new List<double>(genetics.Count);

            var sortedGenetics = genetics.OrderBy(x => x.EvaluationValue).ToList();
            var evaluSum = sortedGenetics.Sum(x => x.EvaluationValue);
            var probabilityPreparation = sortedGenetics.Select(x => evaluSum / x.EvaluationValue).ToList();

            _sum = probabilityPreparation.Sum();

            double countup = 0;

            foreach (var value in probabilityPreparation)
            {
                countup += value;
                _lookupTable.Add(countup);
            }
        }


        public int SelectIdx()
        {
            var randomValue = _random.NextDouble() * _sum;
            (_, var index) = _lookupTable.Select((sorce, idx) => (sorce, idx))
                                    .Last(x => randomValue < x.sorce);
            return index;
        }
    }
}