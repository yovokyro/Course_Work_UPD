    using Miner.Object;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Класс описывает связь логики класса BoomEffect и отображаемого спрайта
    /// </summary>
    public class BoomEffectGame : RenderObject
    {
        private float _time;
        //Время отображения спрайта взрыва
        public float Time { get => _time; set => _time = value; }

        private int _stage;
        //стадия взрыва (номер спрайта)
        public int Stage { get => _stage; set => _stage = value; }
        public BoomEffectGame(BoomEffect boom, Sprite sprite, float scale) : base(boom, sprite)
        {
            _time = 0;
            _stage = 0;
            base.scale = scale;
        }
    }
}
