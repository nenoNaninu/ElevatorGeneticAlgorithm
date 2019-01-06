using System;
using System.Collections.Generic;
using System.Linq;
using ElevatorGeneticAlgorithm.Repository;

namespace ElevatorGeneticAlgorithm.Model
{
    public enum MoveDirection
    {
        GoAbove,
        GoBottom
    }

    public class Elevator
    {
        private List<Person> _carryingPeople;
        public int CurrentFloor { get; set; }
        public int NextFloor { get; set; }
        public double Speed { get; set; }

        public int MaxCarryingNum { get; }

        public double Timer { get; private set; } = 0;

        public MoveDirection Direction { get; private set; } = MoveDirection.GoAbove;

        public Elevator(int maxCarrying)
        {
            MaxCarryingNum = maxCarrying;
            _carryingPeople = new List<Person>(maxCarrying);
        }

        /// <summary>
        /// エレベータに乗る関数。
        /// ここまで動いている間に降ろせるやつは降ろす。
        /// のったタイミングのタイムスタンプをそれぞれのPersonに押す。
        /// </summary>
        /// <param name="person"></param>
        /// <returns>あらゆる理由で乗れなかった場合はfalseを返す。</returns>
        public bool GetOn(Person person)
        {
            //乗客がいない場合はその人間の行きたい方向に決まる。
            if (!_carryingPeople.Any())
            {
                Timer = person.StartWaitingTime;//エレベータが動き始める時間は
                Timer += Math.Abs(CurrentFloor - person.CurrentFloor) * Database.Configuration.ElevatorSpeed;
                Direction = person.Direction;
                _carryingPeople.Add(person);
                person.TakeElevatorTime = Timer;
                return true;
            }

            //personが待ち始める時間がいまのエレベータの時間より前だったとき。
            //要するに未来の人間だったとき。
            while (Timer < person.StartWaitingTime && _carryingPeople.Any())
            {
                if (Direction == MoveDirection.GoAbove)
                {
                    var min = _carryingPeople.Min(x => x.TargetFloor);
                    var getoffPeople = _carryingPeople.Where(x => x.TargetFloor == min).ToList();
                    Timer += (min - CurrentFloor) * Database.Configuration.ElevatorSpeed;
                    GetOff(getoffPeople);
                }
                else
                {
                    var max = _carryingPeople.Max(x => x.TargetFloor);
                    var getoffPeople = _carryingPeople.Where(x => x.TargetFloor == max).ToList();
                    Timer += (CurrentFloor - max) * Database.Configuration.ElevatorSpeed;
                    GetOff(getoffPeople);
                }
            }

            //それでも未来人だった場合
            if (Timer < person.StartWaitingTime)
            {
                return false;
            }

            //もしくはエレベータが通り過ぎてしまった場合
            if (Direction == MoveDirection.GoAbove)
            {
                if (person.CurrentFloor < CurrentFloor)
                {
                    return false;
                }
            }
            else
            {
                if (CurrentFloor < person.CurrentFloor)
                {
                    return false;
                }
            }

            //personの階に行くまでに降りる人たちについて。
            if (Direction == MoveDirection.GoAbove)
            {
                var getoffPeople = _carryingPeople.Where(x => x.TargetFloor < person.CurrentFloor).ToList();//遅延評価の参照の握りがよくわからんのであとで検証。
                GetOff(getoffPeople);
            }
            else
            {
                var getoffPeople = _carryingPeople.Where(x => person.CurrentFloor < x.TargetFloor).ToList();//遅延評価の参照の握りがよくわからんのであとで検証。
                GetOff(getoffPeople);
            }

            //personがいる階に到着。
            Timer += Math.Abs(CurrentFloor - person.CurrentFloor) * Database.Configuration.ElevatorSpeed;
            CurrentFloor = person.CurrentFloor;

            var getoffSameFloorPeople = _carryingPeople.Where(x => person.CurrentFloor == x.TargetFloor).ToList();//遅延評価の参照の握りがよくわからんのであとで検証。
            if (0 < getoffSameFloorPeople.Count)
            {
                GetOff(getoffSameFloorPeople);
            }
            else
            {
                //降りるやつがいなくてもどっちにしろ開けないといけない。
                Timer += Database.Configuration.OpenDoorTime;
            }

            if (_carryingPeople.Count > MaxCarryingNum)
            {
                _carryingPeople.Add(person);
                person.TakeElevatorTime = Timer;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 全員降ろす。
        /// 全員降ろした後はdirectionがひっくり返る。
        /// </summary>
        public void GetOffAllPerson()
        {
            if (Direction == MoveDirection.GoAbove)
            {
                var top = _carryingPeople.Max(x => x.TargetFloor);
                Timer += (top - CurrentFloor) * Database.Configuration.ElevatorSpeed;
                GetOff(_carryingPeople);
                CurrentFloor = top;
                Direction = MoveDirection.GoBottom;
            }
            else
            {
                var bottom = _carryingPeople.Min(x => x.TargetFloor);
                Timer += (CurrentFloor - bottom) * Database.Configuration.ElevatorSpeed;
                GetOff(_carryingPeople);
                CurrentFloor = bottom;
                Direction = MoveDirection.GoAbove;
            }
        }


        public void Move()
        {
            //上に行く場合
            if (Direction == MoveDirection.GoAbove)
            {

            }
            else//下に行く場合
            {

            }
        }

        public void GetOff(Person person)
        {
            _carryingPeople.Remove(person);
        }

        /// <summary>
        /// 降りる人間のリストをぶち込むと降りるときに開いている扉の時間を考慮したりしつつ降ろす。
        /// </summary>
        /// <param name="getoffPeople"></param>
        public void GetOff(List<Person> getoffPeople)
        {
            var stopFloorNum = getoffPeople.Select(x => x.TargetFloor).Distinct().Count();
            Timer += stopFloorNum * Database.Configuration.OpenDoorTime;

            foreach (var getoffPerson in getoffPeople)
            {
                _carryingPeople.Remove(getoffPerson);
            }
        }
    }
}