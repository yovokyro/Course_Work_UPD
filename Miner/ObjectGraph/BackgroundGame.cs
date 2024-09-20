using Miner.Object;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Класс описывает связь логики класса Background и отображаемого спрайта
    /// </summary>
    public class BackgroundGame : RenderObject
    {
        public BackgroundGame(Background background, Sprite sprite) : base(background, sprite) 
        {
            scale = 1;
        }
    }
}
