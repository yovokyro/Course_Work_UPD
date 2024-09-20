using Miner.Object;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Класс описывает связь логики класса Mine и отображаемого спрайта
    /// </summary>
    public class WallGame : RenderObject
    {
        private Object.Wall _wall;
        //объект класса Mine
        public Object.Wall Wall { get => _wall; }
        public WallGame(Object.Wall wall, Sprite sprite) : base(wall, sprite) 
        {
            _wall = wall;
            scale = 1;
        }
    }
}
