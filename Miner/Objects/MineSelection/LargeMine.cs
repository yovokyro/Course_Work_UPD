using Miner.Object.Enum;

namespace Miner.Object.MineSelection
{
    /// <summary>
    /// Класс представляет собой сильную мину
    /// </summary>
    public class LargeMine : MineDecorator
    {
        public LargeMine(Mine mine) : base(mine)
        {
            radius = 5;
            type = MineType.Large;
        }
    }
}
