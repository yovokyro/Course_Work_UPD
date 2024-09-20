namespace Miner.Object.MineSelection
{
    /// <summary>
    /// Абстрактный класс декоратора мины
    /// </summary>
    public abstract class MineDecorator : Mine
    {
        protected MineDecorator(Mine mine) : base(mine.Position)
        {
        }
    }
}
