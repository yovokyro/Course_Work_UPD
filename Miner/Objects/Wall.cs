using SharpDX;
using Miner.Object.Interfaces;
using Miner.Object.Enum;

namespace Miner.Object
{
    /// <summary>
    /// Класс представляет игровой объект Wall, реализует интерфейс IDamageWall
    /// </summary>
    public class Wall: ObjectGame, IDamageWall
    {
        private int _health;
        //здоровье стены
        public int Health { get => _health; }
        private bool _destructibility;
        //статус разрушаемости стены
        public bool Destructibility { get => _destructibility; }

        private WallType _type;
        //тип стены
        public WallType Type { get => _type; }
        public Wall(Vector2 position, int health, bool destructibility, WallType type) : base(position) 
        {
            _health = health;
            _type = type;
            _destructibility = destructibility;
        }

        /// <summary>
        /// Нанесение урона стене
        /// </summary>
        public void Damage() //если мина задела стену
        {
            if(_destructibility)
                _health--;
        }

        /// <summary>
        /// Возвращает статус жизни игрока
        /// </summary>
        /// <returns>Возвращает статус жизни игрока</returns>
        public bool GetLive()
        {
            if (_health > 0)
                return true;

            else return false;
        }
    }
}
