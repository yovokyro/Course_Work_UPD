using SharpDX;

namespace Miner.Object
{
    /// <summary>
    /// Класс реализующий спрайт, его размер, угол поворота и центрирование
    /// </summary>
    public class Sprite
    {
        private float _rotation;
        //угол поворота спрайта
        public float Rotation { get => _rotation; set => _rotation = value; }

        private int _spriteId;
        //номер спрайта
        public int SpriteId { get => _spriteId; }

        private float _width;
        //ширина спрайта
        public float Width { get => _width; }
        private float _height;
        //высота спрайта
        public float Heigth { get => _height; }

        private Vector2 _center;
        //центр спрайта
        public Vector2 Center { get => _center; }
        public Sprite(int spriteId, int width, int heigth, float rotation)
        {
            _spriteId = spriteId;
            _rotation = rotation;
            _width = width;
            _height = heigth;

            _center.X = _width / 2.0f;
            _center.Y = _height / 2.0f;
        }

        /// <summary>
        /// Смена спрайта
        /// </summary>
        /// <param name="spriteId">Номер изменяемого спрайта</param>
        public void ReplaceSprite(int spriteId)
        {
            try
            {
                _spriteId = spriteId;
            }
            catch { }
        }

        /// <summary>
        /// Устанавливает центр спрайта для вращения
        /// </summary>
        /// <param name="size">Величина смещения центра</param>
        public void SetCenterRotation(float size)
        {
            _center += size;
        }
    }
}
