using System;

namespace ElevatorGeneticAlgorithm.Model
{
    public class Person : IEquatable<Person>
    {
        public int Id { get; set; }

        /// <summary>
        /// エレベータに並び始めた時間
        /// </summary>
        public double StartWaitingTime { get; set; }

        public int CurrentFloor { get; set; }

        /// <summary>
        /// 目的の階数
        /// </summary>
        public int TargetFloor { get; set; }

        /// <summary>
        /// エレベータに乗った時間
        /// </summary>
        public double TakeElevatorTime { get; set; }

        public MoveDirection Direction => CurrentFloor < TargetFloor
            ? MoveDirection.GoAbove
            : MoveDirection.GoBottom;

        /// <summary>
        /// 待っていた間の時間
        /// </summary>
        public double WaitingTime => TakeElevatorTime - StartWaitingTime;

        public bool Equals(Person other)
        {
            return Id == other.Id;
        }
    }
}