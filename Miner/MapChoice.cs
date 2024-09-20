using System;
using System.Drawing;
using System.IO;
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
    }
}
