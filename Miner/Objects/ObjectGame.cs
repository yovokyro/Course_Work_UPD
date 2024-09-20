using SharpDX;

namespace Miner.Object
{
    /// <summary>
    /// Абстрактный класс игрового объекта. Отвечает за позиционирования объекта на игровом поле
    /// </summary>
    public abstract class ObjectGame
    {
        //позиционирование относительно игрового поля
        protected Vector2 position;
        public Vector2 Position { get => position;}
        public ObjectGame(Vector2 position)
        {
            this.position = position;
        }
    }
}
