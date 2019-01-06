namespace ElevatorGeneticAlgorithm.Model
{
    public class Configuration
    {
        /// <summary>
        /// エレベータにやってくる人数(決め打ち)
        /// </summary>
        public int TotalPeopleNumber { get; set; } = 100;

        /// <summary>
        /// 遺伝子長
        /// </summary>
        public int GenericNumber { get; set; } = 100;

        /// <summary>
        /// ビルの一番上の回数。
        /// </summary>
        public int TopFloor { get; set; } = 20;

        /// <summary>
        /// 最大積載人数
        /// </summary>
        public int MaxLoadingNum { get; set; } = 10;

        /// <summary>
        /// エレベータの扉が開いている時間。
        /// </summary>
        public double OpenDoorTime { get; set; } = 10;

        /// <summary>
        /// 1階進むときにかかる時間。
        /// </summary>
        public double ElevatorSpeed { get; set; } = 0.5;

        /// <summary>
        /// 規定値では人を5分間の間流れてくると仮定。
        /// </summary>
        public double SamplingTimeSpan { get; set; } = 5 * 60;
    }
}