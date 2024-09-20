using SharpDX;
using Miner.Object.Enum;

namespace Miner.Object
{
    /// <summary>
    /// Класс игроого объекта Mine
    /// </summary>
    public class Mine : ObjectGame
    {
        protected int radius; 
        //радиус действия мины
        public int Radius { get => radius; }
        protected MineType type; 
        //тип мины
        public MineType Type { get => type; }
        public Mine(Vector2 position) : base(position)
        {
        }

    }
}
