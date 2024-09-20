using Miner.Object.Enum;
using System.Drawing;

namespace Miner
{
    /// <summary>
    /// Структура, описывающая цвет пикселя и возвращающаяя соответствующий тип стены по цвету
    /// </summary>
    public struct PixelsColorStruct
    {
        //красный цвет
        public byte r;
        //зеленый цвет
        public byte g;
        //синий цвет
        public byte b;

        /// <summary>
        /// Присваивание структуре определенный цвет
        /// </summary>
        /// <param name="color">Присваиваемый цвет</param>
        public void SetColor(Color color)
        {
            r = color.R;
            g = color.G;
            b = color.B;
        }

        /// <summary>
        /// Возвращает тип стены по цвету
        /// </summary>
        /// <returns>Возвращает тип стены по цвету</returns>
        public WallType GetTypeWall()
        {
            switch($"{r}{g}{b}")
            {
                case "255255255":
                    return WallType.None;
                case "192192192":
                    return WallType.Weak;
                case "128128128":
                    return WallType.Medium;
                case "000":
                    return WallType.Endless;
                default:
                    return WallType.None;
            }
        }
    }
}
