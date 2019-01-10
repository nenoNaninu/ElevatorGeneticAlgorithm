using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ElevatorGeneticAlgorithm.Model;
using ElevatorGeneticAlgorithm.Repository;

namespace ElevatorGeneticAlgorithm
{
    public static class GeneticAlgorithm
    {

        public static async Task Learning(int iterationCount, List<Genetic> genetics, List<Person> peoples, int pairNumberOfCrossoverParents, double mutationRate,int genericNumber, int maxCarrying, double elevatorSpeed, double openDoorSpeed)
        {
            for (int i = 0; i < iterationCount; i++)
            {
                //遺伝的アルゴリズム
                
                genetics = CirculationCrossover(genetics, pairNumberOfCrossoverParents, mutationRate);

                foreach ((var genetic, var gIdx) in genetics.Select((item, idx) => (item, idx)))
                {
                    EvaluateGenetic(genetic, peoples, maxCarrying, elevatorSpeed, openDoorSpeed);
                    Console.WriteLine($"Iteration: {i,3:D} Genetic: {gIdx,3:D} Evaluate: {genetic.EvaluationValue}");
                }

                //淘汰方法： ルーレット選択．ただし，上位 20 個はエリート選択で選ぶものとする
                genetics = RouletteSelection(genetics, genericNumber, 20);

                //保存。
                await Database.SaveGenetic(genetics, i);
            }
        }

        /// <summary>
        /// 交配方法は順序交叉
        /// 突然変異は逆位
        /// </summary>
        private static List<Genetic> CirculationCrossover(List<Genetic> genetics, int pairNumberOfCrossoverParents, double mutationRate)
        {
            var newGeneticList = new List<Genetic>(genetics);
            var rand = new Random();

            for (int i = 0; i < pairNumberOfCrossoverParents; i++)
            {
                Console.WriteLine($"CirculationCrossover: {i,3:D}");
                //親の選択
                var count = newGeneticList.Count;
                var parentIdx1 = rand.Next(0, count - 1);
                var parentIdx2 = rand.Next(0, count - 1);

                while (parentIdx1 == parentIdx2)
                {
                    parentIdx2 = rand.Next(0, count - 1);
                }

                //突然変異
                if (mutationRate < rand.NextDouble())
                {
                    var mutatedGenetic1 = InversionMutate(newGeneticList[parentIdx1]);
                    var mutatedGenetic2 = InversionMutate(newGeneticList[parentIdx2]);

                    newGeneticList.Add(mutatedGenetic1);
                    newGeneticList.Add(mutatedGenetic2);

                    continue;
                }

                var parent1 = newGeneticList[parentIdx1];
                var parent2 = newGeneticList[parentIdx2];

                var child1 = CirculationCrossoverHelper(parent1, parent2);
                var child2 = CirculationCrossoverHelper(parent2, parent1);

                newGeneticList.Add(child1);
                newGeneticList.Add(child2);

            }

            return newGeneticList;
        }

        private static Genetic CirculationCrossoverHelper(Genetic parent1, Genetic parent2)
        {
            var rand = new Random();
            //切れ目の選択
            var partation = rand.Next(0, parent1.Length - 1);

            var left = parent1.Where((_, idx) => idx < partation).ToList();
            var right = new List<int>();

            foreach (var id in parent2)
            {
                if (!left.Contains(id))
                {
                    right.Add(id);
                }
            }

            left.AddRange(right);
            return new Genetic(left);
        }


        /// <summary>
        /// 突然変異(逆位)
        /// </summary>
        private static Genetic InversionMutate(Genetic genetic)
        {
            var random = new Random();

            var newGenetic = new Genetic(genetic);

            var length = newGenetic.Length;

            var idx1 = random.Next(0, length - 1);
            var idx2 = random.Next(0, length - 1);

            var tmp = newGenetic[idx1];
            newGenetic[idx1] = newGenetic[idx2];
            newGenetic[idx2] = tmp;

            return newGenetic;
        }


        public static void EvaluateGenetic(Genetic genetic, List<Person> people, int maxCarrying, double elevatorSpeed, double openDoorSpeed)
        {
            Elevator.Simulate(genetic, people, maxCarrying, elevatorSpeed, openDoorSpeed);
            genetic.Evaluate(EvaluateMethod.MinimizeSigmoidWaitingTime, people);

            foreach (var person in people)
            {
                person.TakeElevatorTime = double.MaxValue;
            }
        }


        /// <summary>
        /// ルーレット方式で淘汰
        /// </summary>
        /// <param name="genetics">評価済みのgeneticリスト</param>
        /// <param name="genericNumber"></param>
        /// <param name="eliteNumber">ルーレット回す前に次世代になることが確約された遺伝子の数。</param>
        /// <returns></returns>
        private static List<Genetic> RouletteSelection(List<Genetic> genetics,int genericNumber,int eliteNumber)
        {
            var newGeneticList = genetics.OrderBy(g => g.EvaluationValue)
                .Where((_, idx) => idx < eliteNumber).ToList();

            var stock = genetics.OrderBy(g => g.EvaluationValue)
                .Where((_, idx) => eliteNumber <= idx).ToList();

            var roulette = new Roulette(stock);

            while(newGeneticList.Count >= genericNumber)
            {
                var idx = roulette.SelectIdx();
                var genetic = stock[idx];
                if (newGeneticList.Contains(genetic))
                {
                    continue;
                }

                newGeneticList.Add(genetic);
            }

            return newGeneticList;
        }
    }
}