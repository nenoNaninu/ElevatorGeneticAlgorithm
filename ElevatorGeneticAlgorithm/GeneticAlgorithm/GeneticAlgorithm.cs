using System.Collections.Generic;
using System.Linq;
using ElevatorGeneticAlgorithm.Model;
using ElevatorGeneticAlgorithm.Repository;

namespace ElevatorGeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        /// <summary>
        /// 目的関数
        /// </summary>
        public static void ObjectiveFunction()
        {

        }

        private static void Simulate(Genetic genetic, List<Person> peopleList)
        {
            var elevator = new Elevator(Database.Configuration.MaxLoadingNum);

            //各遺伝子について。
            foreach (int id in genetic)
            {
                var person = peopleList.First(p => p.Id == id);

                //エレベータが上に動いているのに客がエレベータより下にいるのに上に行くからと言って
                //下に戻るのはおかしい。なので制限。
                if (elevator.Direction == MoveDirection.GoAbove)
                {
                    if (person.CurrentFloor < elevator.CurrentFloor)
                    {
                        elevator.GetOffAllPerson();
                        elevator.GetOn(person);
                    }
                }
                else
                {
                    if (elevator.CurrentFloor < person.CurrentFloor)
                    {
                        elevator.GetOffAllPerson();
                        elevator.GetOn(person);
                    }
                }

                //エレベータの動いている方向と、向かいたい方向が一致していた場合にのみ
                if (person.Direction == elevator.Direction)
                {
                    bool canGetOn = elevator.GetOn(person);

                    //載せられなかったら動いて降ろしていくほかない
                    if (!canGetOn)
                    {
                        elevator.GetOffAllPerson();
                        elevator.GetOn(person);
                    }
                }
                else//動いている方向と逆だったら、いまいる全乗客が下りるまで稼働。そのあとに乗る。
                {
                    elevator.GetOffAllPerson();
                    elevator.GetOn(person);
                }
            }
        }

        public static void Learning(int iterationCount, List<Genetic> genetics, List<Person> peoples)
        {
            for (int i = 0; i < iterationCount; i++)
            {
                //まずは評価するまえに評価できるだけの準備をする。
                var peopleList = new List<Person>(peoples);

                foreach (var genetic in genetics)
                {

                    //シミュレーション
                    Simulate(genetic, peopleList);


                    //この段階で今の遺伝子がどれくらいの評価なのか評価。

                    //その後汚れたリストの中を綺麗にする。
                    foreach (var person in peoples)
                    {
                        person.TakeElevatorTime = 0;
                    }

                }

                //評価

                //ここで遺伝的アルゴリズムがさく裂。
                //循環交換と突然変異だけでいいや。

            }
        }
    }
}