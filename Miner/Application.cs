using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using Miner.DirectX;
using Miner.Object;
using Miner.ObjectGraph;
using Miner.Object.MineSelection;
using Miner.Object.Enum;
using System;
using UPDController;
using System.Threading.Tasks;
using System.Linq;

namespace Miner
{
    /// <summary>
    /// Основной класс игры. Класс содержит связь логики и графики
    /// </summary>
    public class Application
    {

        private ISocket _socket;

        private bool _game;
        private string _win;
        //вывод сообщения об завершении игры
        public string Win { get => _win; }

        //задержка перед завершением игры
        private float _delay;
        //задержка смены спрайтов персонажа
        private float _playerOneDelay;
        //задержка смены спрайтов персонажа
        private float _playerTwoDelay;
        //задержка воспроизведения звука ходьбы
        private float _strokeDelay;

        //размер объектов
        private int _mainSize = 60;
        //скорость игрока
        private float _movementSpeed = 3;

        private RenderForm _renderForm;
        private DirectX2D _dx2d;
        private DXInput _dXInput;
        private WindowRenderTarget _target;
        private SoundsControl _soundsControl;
        private Helper _helper;
        private Drawer _drawer;

        private BackgroundGame _background;

        private Wall _wallFactory;
        private Creator _creator;

        private WallType[,] _wallsIndx;
        private List<WallGame> _walls;

        private int _wallSmall;
        private int _wallMedium3;
        private int _wallMedium2;
        private int _wallMedium1;
        private int _wallLarge;

        private PlayerGame _player;
        private PlayerGame _playerTwo;

        private int[] _playerSprite;
        private int[] _playerTwoSprite;
        private bool[] _playerMiningSprite;
        private int _playerDead;

        //отслеживание передвижения игрока при ходьбе для звука
        private Vector2 _positionPlayer;
        private Vector2 _positionPlayerTwo;

        //мина каждого из игроков + эффекты от мин
        private MineGame _mine;
        private MineGame _mineTwo;
        private List<BoomEffectGame> _boomEffect;

        private bool _playerTwoMining = false;

        private List<MineGame> _menuMine;

        private int _mineSmall;
        private int _mineMedium;
        private int _mineLarge;
        private int[] _boom;

        public Application(ISocket socket, PlayerSetting player, float musicVolume, float soundsVolume)
        {
            _socket = socket;

            _game = true;
            _win = "Игра завершена!";

            _renderForm = new RenderForm("Miner");
            _renderForm.WindowState = FormWindowState.Maximized;
            _renderForm.Icon = new System.Drawing.Icon(@"..\..\..\Resources\icon.ico");

            _dx2d = new DirectX2D(_renderForm);
            _dXInput = new DXInput(_renderForm);
            _dx2d.RenderTarget.Resize(new Size2(1920, 1080));
            _target = _dx2d.RenderTarget;

            int background = _dx2d.GetImageLoad(@"..\..\..\Resources\background_game.bmp");
            _background = new BackgroundGame(new Background(new Vector2(0, 0)), new Sprite(background, 0, 0, 0));

            _wallSmall = _dx2d.GetImageLoad(@"..\..\..\Resources\walls\walls1.bmp");
            _wallMedium3 = _dx2d.GetImageLoad(@"..\..\..\Resources\walls\walls2_3.bmp");
            _wallMedium2 = _dx2d.GetImageLoad(@"..\..\..\Resources\walls\walls2_2.bmp");
            _wallMedium1 = _dx2d.GetImageLoad(@"..\..\..\Resources\walls\walls2_1.bmp");
            _wallLarge = _dx2d.GetImageLoad(@"..\..\..\Resources\walls\walls3.bmp");
            _walls = new List<WallGame>();
            WallGeneration();

            _movementSpeed = _mainSize / 20;



            RandomPersonSkin randomSkin = new RandomPersonSkin();

            string[] paths = randomSkin.GetAllDirectories();
            int[] sprite_idx = paths.SelectMany(path => new[]
            {
                _dx2d.GetImageLoad($@"{path}\player_stand.bmp"),
                _dx2d.GetImageLoad($@"{path}\player_mining.bmp")
            }).ToArray();

            _playerSprite = new int[paths.Length];
            _playerDead = _dx2d.GetImageLoad(@"..\..\..\Resources\players\player_dead.bmp");

            Random random = new Random();
            int rd_idx = (_socket.Type == SocketTypes.Client) ? random.Next(paths.Length / 2, paths.Length) : random.Next(0, paths.Length / 2);

            for (int i = 0; i < 2; i++)
            {
                _playerSprite[i] = sprite_idx[rd_idx * 2 + i];
            }


            Dictionary<int, int> emptyMines = new Dictionary<int, int>
            {
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
            };


            if (_socket.Type == SocketTypes.Client)
            {
                _player = new PlayerGame(new Player(new Vector2(_mainSize * 31, _mainSize + (float)(_mainSize * 16)), _movementSpeed), new Sprite(_playerSprite[0], _mainSize - 2, _mainSize - 2, 0), player.Mines, player.Name);
                _playerTwo = new PlayerGame(new Player(new Vector2(_mainSize * 3, _mainSize + (float)(_mainSize * 2)), _movementSpeed), new Sprite(_playerSprite[0], _mainSize - 2, _mainSize - 2, 0), emptyMines, "");
            }
            else
            {
                _player = new PlayerGame(new Player(new Vector2(_mainSize * 3, _mainSize + (float)(_mainSize * 2)), _movementSpeed), new Sprite(_playerSprite[0], _mainSize - 2, _mainSize - 2, 0), player.Mines, player.Name);
                _playerTwo = new PlayerGame(new Player(new Vector2(_mainSize * 31, _mainSize + (float)(_mainSize * 16)), _movementSpeed), new Sprite(_playerSprite[0], _mainSize - 2, _mainSize - 2, 0), emptyMines, "");
            }

            _positionPlayer = _player.ObjectGame.Position;
            _positionPlayerTwo = _playerTwo.ObjectGame.Position;

            _playerMiningSprite = new bool[2];
            _playerMiningSprite[0] = false;
            _playerMiningSprite[1] = false;

            _mineSmall = _dx2d.GetImageLoad(@"..\..\..\Resources\mines\mine1.bmp");
            _mineMedium = _dx2d.GetImageLoad(@"..\..\..\Resources\mines\mine2.bmp");
            _mineLarge = _dx2d.GetImageLoad(@"..\..\..\Resources\mines\mine3.bmp");

            _menuMine = new List<MineGame>
            {
                new MineGame(new SmallMine(new Mine(new Vector2(_mainSize - (float)(_mainSize / 4.5), _mainSize / 4))), new Sprite(_mineSmall, (int)(_mainSize / 2.4),(int)(_mainSize / 2.4), 0), 0, 0.2f),
                new MineGame(new MediumMine(new Mine(new Vector2((float)(_mainSize * 2.4), _mainSize / 4))), new Sprite(_mineMedium,(int)(_mainSize / 2.4),(int)(_mainSize / 2.4), 0), 0, 0.2f),
                new MineGame(new LargeMine(new Mine(new Vector2((float)(_mainSize * 3.9), _mainSize / 4))), new Sprite(_mineLarge, (int)(_mainSize / 2.4),(int)(_mainSize / 2.4), 0), 0, 0.2f),
            };

            _boom = new int[13];
            _boomEffect = new List<BoomEffectGame>();
            for (int i = 0; i < _boom.Length; i++)
                _boom[i] = _dx2d.GetImageLoad($"..\\..\\..\\Resources\\mines\\boom\\boom{i}.bmp");

            SendPlayerInfo();
            GetPlayerInfo();

            _drawer = new Drawer(_dx2d);
            _helper = new Helper();

            _soundsControl = new SoundsControl(musicVolume, soundsVolume);
            _soundsControl.Music();
        }

        /// <summary>
        /// Рендер каждого кадра
        /// </summary>
        private void Render()
        {
            if (_soundsControl.GetMusicRepeat())
                _soundsControl.Music();

            _helper.Tick();

            _dXInput.UpdateKeyboard();

            _target.BeginDraw();
            _target.Clear(Color.Black);
            _drawer.Draw(_background);

            WallsDraw();
            MineDraw();
            PlayerDraw();

            if (_boomEffect.Count != 0)
                BoomDraw();

            Moving();

            StatusMenuDraw();
            End();

            SendPlayerInfo();
            GetPlayerInfo();

            _target.EndDraw();
        }

        /// <summary>
        /// Передвижение игроков
        /// </summary>
        private void Moving()
        {
            if (_dXInput.KeyboardConnect)
            {
                if (_player.GetLive())
                {
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.D1)) _player.GetChooseMina(1);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.D2)) _player.GetChooseMina(2);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.D3)) _player.GetChooseMina(3);

                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.W) && !GetСollision(_player, 0, -_movementSpeed)) _player.Movement(MovementKeys.Up);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.A) && !GetСollision(_player, -_movementSpeed, 0)) _player.Movement(MovementKeys.Left);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.S) && !GetСollision(_player, 0, _movementSpeed)) _player.Movement(MovementKeys.Down);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.D) && !GetСollision(_player, _movementSpeed, 0)) _player.Movement(MovementKeys.Right);
                    PlayerRotation(_player);

                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.LeftShift) && !_player.Installation)
                        if (_player.GetMining())
                        {
                            switch (_player.MineActive)
                            {
                                case 1:
                                    _mine = new MineGame(new SmallMine(new Mine(_player.Rect.Center)), new Sprite(_mineSmall, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), _helper.Time + 1, (float)_mainSize / 350);
                                    break;
                                case 2:
                                    _mine = new MineGame(new MediumMine(new Mine(_player.Rect.Center)), new Sprite(_mineMedium, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), (float)(_helper.Time + 1.5), (float)_mainSize / 350);
                                    break;
                                case 3:
                                    _mine = new MineGame(new LargeMine(new Mine(_player.Rect.Center)), new Sprite(_mineLarge, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), _helper.Time + 2, (float)_mainSize / 350);
                                    break;
                            }

                            _player.Sprite.ReplaceSprite(_playerSprite[1]);
                            _playerOneDelay = _helper.Time + 0.25f;
                            _playerMiningSprite[0] = true;
                            _soundsControl.Mine();
                        }
                }
            }

            StrokePlayer();
        }

        /// <summary>
        /// Вращает спрайт персонажа вокруг его цента по наименьшему углу
        /// </summary>
        /// <param name="player">Игровой персонаж</param>
        private void PlayerRotation(PlayerGame player)
        {
            double rotationDistant = Math.Round(player.RotationNow - player.Rotation, 2);
            if (Math.Round(player.Sprite.Rotation, 2) != Math.Round(player.Rotation, 2))
            {
                player.Speed(true);
                if (player.Time <= _helper.Time)
                {
                    if (rotationDistant == -Math.Round(Math.PI, 2) / 2 || rotationDistant == Math.Round(Math.PI, 2) * 3 / 2 || rotationDistant == Math.Round(Math.PI, 2))
                        player.Sprite.Rotation += (float)(Math.Round(Math.PI, 2) / 20);
                    else
                        player.Sprite.Rotation -= (float)(Math.Round(Math.PI, 2) / 20);

                    player.Time = _helper.Time + 0.01f;

                    if (Math.Round(player.Sprite.Rotation, 2) >= Math.Round(Math.PI, 2) * 2)
                        player.Sprite.Rotation = 0;

                    if (Math.Round(player.Sprite.Rotation, 2) == -Math.Round(Math.PI, 2))
                        player.Sprite.Rotation = (float)Math.Round(Math.PI, 2);

                    if (Math.Round(player.Sprite.Rotation, 2) == -Math.Round(Math.PI, 2) / 2)
                        player.Sprite.Rotation = (float)Math.Round(Math.PI, 2) * 3 / 2;
                }
            }
            else
            {
                player.Speed(false);
                player.RotationNow = player.Sprite.Rotation;
            }
        }

        /// <summary>
        /// Отображает игроков
        /// </summary>
        private void PlayerDraw()
        {
            if (_playerMiningSprite[0] && _playerOneDelay <= _helper.Time && _player.GetLive())
            {
                _player.Sprite.ReplaceSprite(_playerSprite[0]);
                _playerMiningSprite[0] = false;
            }

            _drawer.Draw(_player);
            _drawer.Draw(_playerTwo);
        }

        /// <summary>
        /// Отображает стены
        /// </summary>
        private void WallsDraw()
        {
            foreach (var wall in _walls)
            {
                _drawer.DrawObject(wall);
            }
        }

        /// <summary>
        /// Отображает меню с минами
        /// </summary>
        private void StatusMenuDraw()
        {
            foreach (var mine in _menuMine)
            {
                _drawer.DrawObject(mine);
                RectangleF rect = mine.Rect;
                rect.X += (float)(_mainSize / 4);
                rect.Width += (float)(_mainSize / 2.4);

                switch (mine.Mine.Type)
                {
                    case MineType.Small:
                        _drawer.DrawText(rect, _player.Mines[1]);
                        if (_player.MineActive == 1)
                            _dx2d.RenderTarget.DrawRectangle(mine.Rect, _dx2d.Brush);
                        break;
                    case MineType.Medium:
                        _drawer.DrawText(rect, _player.Mines[2]);
                        if (_player.MineActive == 2)
                            _dx2d.RenderTarget.DrawRectangle(mine.Rect, _dx2d.Brush);
                        break;
                    case MineType.Large:
                        _drawer.DrawText(rect, _player.Mines[3]);
                        if (_player.MineActive == 3)
                            _dx2d.RenderTarget.DrawRectangle(mine.Rect, _dx2d.Brush);
                        break;
                }
            }
        }

        /// <summary>
        /// Отображает мины
        /// </summary>
        private void MineDraw()
        {
            if (_player.Installation && _helper.Time <= _mine.Time)
                _drawer.Draw(_mine);
            else
            if (_player.Installation)
            {
                _player.Boom();
                _boomEffect.Add(new BoomEffectGame(new BoomEffect(_mine.Rect.Center), new Sprite(_boom[0], (int)(_mainSize * 1.5) * _mine.Mine.Radius, (int)(_mainSize * 1.5) * _mine.Mine.Radius, 0), (float)_mainSize / 333 * _mine.Mine.Radius));
                BoomDraw();
                _soundsControl.Boom();
            }


            if (_playerTwo.Installation && _helper.Time <= _mineTwo.Time)
                _drawer.Draw(_mineTwo);
            else
            if (_playerTwo.Installation)
            {
                _playerTwoMining = false;

                _playerTwo.Boom();
                _boomEffect.Add(new BoomEffectGame(new BoomEffect(_mineTwo.Rect.Center), new Sprite(_boom[0], (int)(_mainSize * 1.5) * _mineTwo.Mine.Radius, (int)(_mainSize * 1.5) * _mineTwo.Mine.Radius, 0), (float)_mainSize / 333 * _mineTwo.Mine.Radius));

                BoomDraw();
                _soundsControl.Boom();
            }
        }

        /// <summary>
        /// Отображает спрайты взрыва
        /// </summary>
        private void BoomDraw()
        {
            List<BoomEffectGame> boomLive = new List<BoomEffectGame>();
            foreach (var boom in _boomEffect)
            {
                if (boom.Stage == 0 && boom.Time == 0)
                {
                    boom.Time = _helper.Time + 0.02f;

                    PlayerKill(boom);
                    DeleteWall(boom);
                }

                if (boom.Time <= _helper.Time)
                {
                    switch (boom.Stage)
                    {
                        case 0:
                            boom.Sprite.ReplaceSprite(_boom[1]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 1:
                            boom.Sprite.ReplaceSprite(_boom[2]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 2:
                            boom.Sprite.ReplaceSprite(_boom[3]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 3:
                            boom.Sprite.ReplaceSprite(_boom[4]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 4:
                            boom.Sprite.ReplaceSprite(_boom[5]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 5:
                            boom.Sprite.ReplaceSprite(_boom[6]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 6:
                            boom.Sprite.ReplaceSprite(_boom[7]);
                            boom.Time = _helper.Time + 0.025f;
                            break;
                        case 7:
                            boom.Sprite.ReplaceSprite(_boom[8]);
                            boom.Time = _helper.Time + 0.05f;
                            break;
                        case 8:
                            boom.Sprite.ReplaceSprite(_boom[9]);
                            boom.Time = _helper.Time + 0.05f;
                            break;
                        case 9:
                            boom.Sprite.ReplaceSprite(_boom[10]);
                            boom.Time = _helper.Time + 0.07f;
                            break;
                        case 10:
                            boom.Sprite.ReplaceSprite(_boom[11]);
                            boom.Time = _helper.Time + 0.09f;
                            break;
                        case 11:
                            boom.Sprite.ReplaceSprite(_boom[12]);
                            boom.Time = _helper.Time + 0.1f;
                            break;
                    }

                    boom.Stage++;
                }

                if (boom.Stage != 13 && boom.Time >= _helper.Time)
                {
                    _drawer.Draw(boom);
                    boomLive.Add(boom);
                }
            }
            _boomEffect = boomLive;
        }

        /// <summary>
        /// Звуки передвижения игроков
        /// </summary>
        private void StrokePlayer()
        {
            if (_strokeDelay <= _helper.Time && _positionPlayer != _player.ObjectGame.Position)
            {
                _soundsControl.Step();
                _strokeDelay = _helper.Time + 0.35f;
                _positionPlayer = _player.ObjectGame.Position;
            }

            if (_strokeDelay <= _helper.Time && _positionPlayerTwo != _playerTwo.ObjectGame.Position)
            {
                _soundsControl.Step();
                _strokeDelay = _helper.Time + 0.35f;
                _positionPlayerTwo = _playerTwo.ObjectGame.Position;
            }
        }

        /// <summary>
        /// Реализует коллизию игрока со стенами лабиринта
        /// </summary>
        /// <param name="player">Игровой персонаж</param>
        /// <param name="x">Смещение при передвижении по оси X</param>
        /// <param name="y">Смещение при передвижении по оси Y</param>
        /// <returns>Возращает статус столконовения игрока со стеной</returns>
        private bool GetСollision(RenderObject player, float x, float y)
        {
            foreach (var wall in _walls)
            {
                RectangleF playerPoly = new RectangleF(player.Rect.Center.X + x, player.Rect.Center.Y + y, player.Rect.Width, player.Rect.Height);

                if (playerPoly.Intersects(new RectangleF(wall.Rect.Center.X, wall.Rect.Center.Y, wall.Rect.Width - 1, wall.Rect.Height - 1)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Нанесение ущерба стенам и их уничтожение
        /// </summary>
        /// <param name="boom">Эффект взрыва мины</param>
        private void DeleteWall(RenderObject boom)
        {
            List<WallGame> wallDead = new List<WallGame>();
            foreach (var wall in _walls)
            {
                if (boom.Rect.Intersects(wall.Rect))
                {
                    wall.Wall.Damage();

                    if (wall.Wall.Type == WallType.Medium)
                    {
                        switch (wall.Wall.Health)
                        {
                            case 2:
                                wall.Sprite.ReplaceSprite(_wallMedium2);
                                break;
                            case 1:
                                wall.Sprite.ReplaceSprite(_wallMedium1);
                                break;
                        }

                        if (!wall.Wall.GetLive())
                            _soundsControl.WallStoneDamage();
                        else
                            _soundsControl.WallStonePartDamage();
                    }
                    else
                        _soundsControl.WallWoodDamage();
                }
                if (wall.Wall.GetLive() || !wall.Wall.Destructibility)
                    wallDead.Add(wall);
            }
            _walls.Clear();
            _walls = wallDead;
        }

        /// <summary>
        /// Генерирование стен лабиринта, используя фабричный метод
        /// </summary>
        private void WallGeneration()
        {
            MapChoice mc = new MapChoice();

            if (_socket is Client clientSocket)
            {
                while (clientSocket.MapInfo == "")
                { }

                _wallsIndx = mc.WallParce(clientSocket.MapInfo);
            }
            else
            {
                _wallsIndx = mc.GetWalls();

                if (_socket is Server server)
                {
                    server.SendMessageAsync($"MapInfo {mc.GetWallsToString()}");
                }
            }

            for (int x = 0; x < _wallsIndx.GetLength(0); x++)
                for (int y = 0; y < _wallsIndx.GetLength(1); y++)
                {
                    switch (_wallsIndx[x, y])
                    {
                        case WallType.None:
                            break;
                        case WallType.Weak:
                            _wallFactory = new WallWeak(y * _mainSize, x * _mainSize, _mainSize, _wallSmall);
                            _creator = _wallFactory.Create();
                            _walls.Add(_creator.CreateMap());
                            break;
                        case WallType.Medium:
                            _wallFactory = new WallMedium(y * _mainSize, x * _mainSize, _mainSize, _wallMedium3);
                            _creator = _wallFactory.Create();
                            _walls.Add(_creator.CreateMap());
                            break;
                        case WallType.Endless:
                            _wallFactory = new WallEndless(y * _mainSize, x * _mainSize, _mainSize, _wallLarge);
                            _creator = _wallFactory.Create();
                            _walls.Add(_creator.CreateMap());
                            break;
                        default:
                            break;
                    }
                }

        }

        /// <summary>
        /// Возвращает статус убийства игрока
        /// </summary>
        /// <param name="boom">Эффект взрыва мины</param>
        /// <returns>Возвращает статус убийства игрока</returns>
        private void PlayerKill(RenderObject boom)
        {
            if (boom.Rect.Intersects(_player.Rect))
            {
                _player.Kill();
                _player.Sprite.ReplaceSprite(_playerDead);
                _soundsControl.Dead();
            }

            if (boom.Rect.Intersects(_playerTwo.Rect))
            {
                _playerTwo.Kill();
                _playerTwo.Sprite.ReplaceSprite(_playerDead);
                _soundsControl.Dead();
            }
        }

        public void SendPlayerInfo()
        {
            string playerInfo = _player.Convert();
            _socket.SendMessageAsync($"PlayerInfo {playerInfo}");
        }

        public void GetPlayerInfo()
        {
            if (_socket.PlayerInfo != "")
            {
                _playerTwo.Parse(_socket.PlayerInfo);

                if (_playerTwo.Installation && !_playerTwoMining)
                {
                    switch (_playerTwo.MineActive)
                    {
                        case 1:
                            _mineTwo = new MineGame(new SmallMine(new Mine(_playerTwo.Rect.Center)), new Sprite(_mineSmall, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), _helper.Time + 1, (float)_mainSize / 350);
                            break;
                        case 2:
                            _mineTwo = new MineGame(new MediumMine(new Mine(_playerTwo.Rect.Center)), new Sprite(_mineMedium, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), (float)(_helper.Time + 1.5), (float)_mainSize / 350);
                            break;
                        case 3:
                            _mineTwo = new MineGame(new LargeMine(new Mine(_playerTwo.Rect.Center)), new Sprite(_mineLarge, (int)(_mainSize / 2), (int)(_mainSize / 2), 0), _helper.Time + 2, (float)_mainSize / 350);
                            break;
                    }

                    _soundsControl.Mine();
                    _playerTwoMining = true;
                }
            }

        }

        /// <summary>
        /// Точка старта приложения. Запускает окно с игрой
        /// </summary>
        public void Run()
        {
            RenderLoop.Run(_renderForm, Render);
        }

        /// <summary>
        /// Точка завершения приложения
        /// </summary>
        /// <returns>Возвращает статус завершения игры</returns>
        public bool End()
        {
            if (!_player.GetInGame() && !_playerTwo.GetInGame() && _player.GetLive() && _playerTwo.GetLive())
            {
                _win = $"Ничья!\nУ игроков закончились мины";
                _game = false;
            }

            if (!_player.GetLive() && !_playerTwo.GetLive())
            {
                _win = $"Ничья!\nВсе игроки погибли";
                _game = false;
            }
            else
             if (!_player.GetLive())
            {
                _win = $"{_playerTwo.Name} победил!";
                _game = false;
            }
            else
            if (!_playerTwo.GetLive())
            {
                _win = $"{_player.Name} победил!";
                _game = false;
            }


            if (!_game && _delay <= _helper.Time && _delay != default)
            {
                _renderForm.Close();
                return true;

            }

            if (!_game && _delay == default)
            {
                _delay = _helper.Time + 1.5f;
            }

            return false;
        }

        /// <summary>
        /// Освобождение неуправляемых ресурсов
        /// </summary>
        public void Dispose()
        {
            _soundsControl.Dispose();
            _dXInput.Dispose();
            _dx2d.Dispose();
            _renderForm.Dispose();
        }
    }
}
