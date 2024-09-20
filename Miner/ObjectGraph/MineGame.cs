using Miner.Object;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Класс описывает связь логики класса Mine и отображаемым спрайтом
    /// </summary>
    public class MineGame : RenderObject
    {
        private float _time;
        //время действия мины
        public float Time { get => _time; }
        private Mine _mine;
        //объект класса Mine
        public Mine Mine { get => _mine; }
        public MineGame(Mine mine, Sprite sprite, float time, float scale) : base(mine, sprite)
        {
            _mine = mine;
            base.scale = scale;
            _time = time;
        }
    }
}
