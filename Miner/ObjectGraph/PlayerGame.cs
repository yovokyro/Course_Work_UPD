using System;
using System.Collections.Generic;
using SharpDX;
using System.Text;
using Miner.Object;
using Miner.Object.Enum;
using Miner.Object.Interfaces;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Класс описывает связь логики класса Player и отображаемого спрайта, реализует интерфейс IMine
    /// </summary>
    public class PlayerGame : RenderObject, IMine
    {
        private Player _player;

        private string _name = "";
        public string Name => _name;

        private float _rotation; 
        //угол поворота спрайта
        public float Rotation { get => _rotation;}
        private float _rotationNow;
        //текущий угол поворота спрайта
        public float RotationNow { get => _rotationNow; set => _rotationNow = value; }
        private float _time;
        //время поворота спрайта
        public float Time { get => _time; set => _time = value; }

        private bool _installation;
        //определяет установленную мину
        public bool Installation { get => _installation; }

        private Dictionary<int, int> _mines;
        //содержит количество мин игрока
        public Dictionary<int, int> Mines { get => _mines; }

        private int _mineActive;
        //определяет выбранный вид мин игрока
        public int MineActive { get => _mineActive; }
        public PlayerGame(Player player, Sprite sprite, Dictionary<int, int> mines, string name) : base(player, sprite)
        {
            _installation = false;
            _mines = mines;
            _name = name;
            ChooseAutoMina();
            RotationCenter();
            _player = player;
            scale = 0.3f;
        }

        /// <summary>
        /// Минирование мины на игровом поле
        /// </summary>
        /// <returns>Возвращает статуст установки мины</returns>
        public bool GetMining()
        {
            if (GetMineCheck() && !_installation)
            {
                if (_mines[_mineActive] == 0)
                    ChooseAutoMina();

                _mines[_mineActive]--;
                _installation = true;

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Автоматический выбор мины
        /// </summary>
        private void ChooseAutoMina()
        {
            if (_mines[1] > 0)
                _mineActive = 1;
            else
               if (_mines[2] > 0)
                _mineActive = 2;
            else
             if (_mines[3] > 0)
                _mineActive = 3;
        }

        /// <summary>
        /// Реализовывает состояние взорванной мины
        /// </summary>
        public void Boom()
        {
            _installation = false;
        }

        /// <summary>
        /// Выбор новой активной мины
        /// </summary>
        /// <param name="id">Номер вида мины</param>
        /// <returns>Возвращает статус выбора мины</returns>
        public bool GetChooseMina(int id)
        {
            if (_mines[id] > 0)
            {
                _mineActive = id;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Передвижение игрока
        /// </summary>
        /// <param name="key">Значение типа MovementKeys, отображающее сторону движения игрока</param>
        public void Movement(MovementKeys key)
        {
            switch (key)
            {
                case MovementKeys.Up:
                    _player.Up();
                    _rotation = 0;
                    break;
                case MovementKeys.Left:
                    _player.Left();
                    _rotation = (float)Math.Round(Math.PI, 2) * 3 / 2;
                    break;
                case MovementKeys.Down:
                    _player.Down();
                    _rotation = (float)Math.Round(Math.PI, 2);
                    break;
                case MovementKeys.Right:
                    _player.Right();
                    _rotation = (float)Math.Round(Math.PI, 2) / 2;
                    break;
            }
        }

        /// <summary>
        /// Проверка наличия мин у игрока
        /// </summary>
        /// <returns>Возвращает статус наличия мин</returns>
        private bool GetMineCheck()
        {

            if (_mines[1] > 0)
                return true;
            else
            if (_mines[2] > 0)
                return true;
            else
            if (_mines[3] > 0)
                return true;
            else
            {
                _mineActive = 0;
                return false;
            }
        }

        /// <summary>
        /// Определяет наличие мин у игрока и статус установленной им мины
        /// </summary>
        /// <returns>Возвращает статус способности игрока к игре</returns>
        public bool GetInGame()
        {
            if (_installation || GetMineCheck())
                return true;

            return false;
        }

        /// <summary>
        /// Убивает игрока
        /// </summary>
        public void Kill()
        {
            sprite.Rotation = 0;
            _player.Kill();
        }

        /// <summary>
        /// Изменяет скорость игрока при вращении
        /// </summary>
        /// <param name="rotation">Статус вращения игрока</param>
        public void Speed(bool rotation)
        {
           _player.Speed(rotation);
        }

        /// <summary>
        /// Возвращает статус жизни игрока
        /// </summary>
        /// <returns>Возвращает статус жизни игрока</returns>
        public bool GetLive()
        {
            return _player.Live;
        }

        /// <summary>
        /// Корректирует центр спрайта
        /// </summary>
        public void RotationCenter()
        {
            Sprite.SetCenterRotation(Rect.Width * 1.2f);
        }

        public string Convert()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{_player.Live};");
            sb.Append($"{_player.Position.X},{_player.Position.Y};");
            sb.Append($"{Rotation};");
            sb.Append($"{RotationNow};");
            sb.Append($"{Time};");
            sb.Append($"{Installation};");
            sb.Append($"{Mines[1]},{Mines[2]},{Mines[3]};");
            sb.Append($"{MineActive};");
            sb.Append($"{Name};");
            sb.Append($"/{sprite.Convert()}");

            return sb.ToString();
        }

        public void Parse(string info)
        {
            string[] split = info.Split('/');

            if (split.Length > 1)
            {
                string[] parts = split[0].Split(';');

                if (parts.Length > 7)
                {
                    if (bool.TryParse(parts[0], out bool live))
                        _player.SetLive(live);

                    string[] positionStr = parts[1].Split(',');

                    Vector2 position = new Vector2(int.Parse(positionStr[0]), int.Parse(positionStr[1]));
                    _player.SetPosition(position);

                    if (float.TryParse(parts[2], out float rotation))
                        _rotation = rotation;

                    if (float.TryParse(parts[3], out float rotationNow))
                        _rotationNow = rotationNow;

                    if (float.TryParse(parts[4], out float time))
                        _time = time;

                    if (bool.TryParse(parts[5], out bool installation))
                        _installation = installation;

                    string[] mines = parts[6].Split(',');

                    _mines[1] = int.Parse(mines[0]);
                    _mines[2] = int.Parse(mines[1]);
                    _mines[3] = int.Parse(mines[2]);

                    if (int.TryParse(parts[7], out int mineActive))
                        _mineActive = mineActive;

                    _name = parts[8];
                }

                sprite.Parse(split[1]);
            }
        }
    }
}
