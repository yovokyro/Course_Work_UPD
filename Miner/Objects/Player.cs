using SharpDX;
using Miner.Object.Interfaces;

namespace Miner.Object
{
    /// <summary>
    /// Класс игрового объекта Player, реализует интерфейс IMovement
    /// </summary>
    public class Player : ObjectGame, IMovement
    {
        private float _slowSpeed;
        private float _normalSpeed;
        private float _movementSpeed;
        //скорость игрока
        public float MovementSpeed { get => _movementSpeed; }
        private bool _live;
        //определяет статус жизни игрока
        public bool Live { get => _live; }

        public Player(Vector2 position, float movementSpeed) : base(position) 
        { 
            _movementSpeed = movementSpeed;
            _normalSpeed = movementSpeed;
            _slowSpeed = movementSpeed - 1;
            _live = true;
        }

        /// <summary>
        /// Изменение скорости игрока при вращении
        /// </summary>
        /// <param name="check">Статус вращения игрока</param>
        public void Speed(bool check)
        {
            if (check && _movementSpeed != _slowSpeed)
                _movementSpeed = _slowSpeed;

            else
                _movementSpeed = _normalSpeed;
        }

        /// <summary>
        /// Передвижение вниз
        /// </summary>
        public void Down()
        {
           position.Y += _movementSpeed;
        }

        /// <summary>
        /// Передвижение влево
        /// </summary>
        public void Left()
        {
            position.X -= _movementSpeed;
        }

        /// <summary>
        /// Передвижение вправо
        /// </summary>
        public void Right()
        {
            position.X += _movementSpeed;
        }

        /// <summary>
        /// Передвижение вверх
        /// </summary>
        public void Up()
        {
            position.Y -= _movementSpeed;
        }

        /// <summary>
        /// Убийство игрока
        /// </summary>
        public void Kill()
        {
            _live = false;
        }

        public void SetLive(bool live) => _live = live;
        public void SetPosition(Vector2 position) => this.position = position;
    }
}
