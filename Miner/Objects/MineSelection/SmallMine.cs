using Miner.Object.Enum;

namespace Miner.Object.MineSelection
{
    /// <summary>
    /// Класс представляет собой слабую мину
    /// </summary>
    public class SmallMine : MineDecorator
    {
        public SmallMine(Mine mine) : base(mine)
        {
            radius = 3;
            type = MineType.Small;
        }
    }
}
