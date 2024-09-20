using Miner.Object.Enum;

namespace Miner.Object.MineSelection
{
    /// <summary>
    /// Класс представляет собой среднюю мину
    /// </summary>
    public class MediumMine : MineDecorator
    {
        public MediumMine(Mine mine) : base(mine)
        {
            radius = 4;
            type = MineType.Medium;
        }
    }
}
