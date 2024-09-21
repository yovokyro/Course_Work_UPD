using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media.Media3D;
using Miner.Object;
using Miner.Object.Enum;

namespace Miner
{
    /// <summary>
    /// Класс отвечает за случайный выбор карты
    /// </summary>
    public class MapChoice
    {
        private string[] _maps { get => GetMaps(); }
        private WallType[,] _wall;
        //двумерный массив стен карты
        public WallType[,] Walls { get => _wall; }
        private Random _random;
        private PixelsColorStruct _colorsStruct;

        public MapChoice() 
        {
            _random = new Random();
            _colorsStruct = new PixelsColorStruct();
        }

        /// <summary>
        /// Возвращает двумерный массив с типом стен
        /// </summary>
        /// <returns>Возвращает двумерный массив с типом стен</returns>
        public WallType[,] GetWalls()
        {
            FileInfo file = new FileInfo(_maps[_random.Next(_maps.Length)]);
            Bitmap bmp = new Bitmap(file.FullName);

            int height = bmp.Height;
            int width = bmp.Width;
            _wall = new WallType[height, width];

            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                {
                    _colorsStruct.SetColor(bmp.GetPixel(x, y));
                    _wall[y, x] = _colorsStruct.GetTypeWall();
                }

            return _wall;
        }

        /// <summary>
        /// Возвращает массив доступных карт из директории
        /// </summary>
        /// <returns>Возвращает массив доступных карт из директории</returns>
        private string[] GetMaps()
        {
            try
            {
               return Directory.GetFiles(@"..\..\..\Resources\maps\");
            }
            catch 
            {
                return null;
            }
        }

        public WallType[,] WallParce(string str)
        {
            string[] parts = str.Split('/');
            string[] sizes = parts[0].Split(',');

            if (sizes.Length > 1 && int.TryParse(sizes[0], out int height) && int.TryParse(sizes[1], out int width))
            {
                _wall = new WallType[height, width];


                int a = 0;
                string walls = parts[1];
                for(int x = 0; x < height; x++)
                    for(int y = 0; y < width; y++)
                    {
                        switch (int.Parse(walls[a].ToString()))
                        {
                            case 0:
                                _wall[x, y] = WallType.None;
                                break;
                            case 1:
                                _wall[x, y] = WallType.Weak;
                                break;
                            case 2:
                                _wall[x, y] = WallType.Medium;
                                break;
                            case 3:
                                _wall[x, y] = WallType.Endless;
                                break;
                            default:
                                break;
                        }
                        a++;
                    }

                return _wall;
            }
            else
                return null;
        }

        public string GetWallsToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{_wall.GetLength(0)},{_wall.GetLength(1)}/");

            for (int x = 0; x < _wall.GetLength(0); x++)
                for (int y = 0; y < _wall.GetLength(1); y++)
                {
                    switch (_wall[x, y])
                    {
                        case WallType.None:
                            sb.Append(0);
                            break;
                        case WallType.Weak:
                            sb.Append(1);
                            break;
                        case WallType.Medium:
                            sb.Append(2);
                            break;
                        case WallType.Endless:
                            sb.Append(3);
                            break;
                        default:
                            break;
                    }
                }

            return sb.ToString();
        }
    }
}
