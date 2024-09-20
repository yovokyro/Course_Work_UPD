using SharpDX;
using SharpDX.Direct2D1;
using Miner.ObjectGraph;

namespace Miner.DirectX
{
    /// <summary>
    /// Класс для отрисовки объектов на игровом поле
    /// </summary>
    public class Drawer
    {
        //DirectX
        private DirectX2D _dx2d;
        //Позиционирование
        private Vector2 _position;

        //Матрицы позиционирования, масштабирования и поворота
        private Matrix3x2 _matrix;
        private Matrix3x2 _oldMatrix;

        public Drawer(DirectX2D dx2d)
        {
            _dx2d = dx2d;
        }

        /// <summary>
        /// Отрисовывает статичные объекты
        /// </summary>
        /// <param name="render">Игровой объект</param>
        public void DrawObject(RenderObject render)
        {
            RectangleF rect = render.Rect;
            _dx2d.RenderTarget.DrawBitmap(_dx2d.Bitmaps[render.Sprite.SpriteId], rect, 1, BitmapInterpolationMode.Linear);
        }

        /// <summary>
        /// Отрисовывает текст, определяющий количество мин
        /// </summary>
        /// <param name="rect">Границы текста</param>
        /// <param name="count">Счет, который необходимо отобразить</param>
        public void DrawText(RectangleF rect, int count)
        {
            if (count == 0)
                _dx2d.RenderTarget.DrawText(count.ToString(), _dx2d.TextFormat, rect, _dx2d.BrushRed);
            else
                _dx2d.RenderTarget.DrawText(count.ToString(), _dx2d.TextFormat, rect, _dx2d.Brush);
        }

        /// <summary>
        /// Отрисовка динамических объектов
        /// </summary>
        /// <param name="render">Игровой объект</param>
        public void Draw(RenderObject render)
        {
            float scale = render.Scale;
            _position.X = (render.ObjectGame.Position.X - render.Sprite.Center.X);
            _position.Y = (render.ObjectGame.Position.Y - render.Sprite.Center.Y); 

            _matrix = Matrix3x2.Rotation(render.Sprite.Rotation, render.Sprite.Center) * Matrix3x2.Scaling(scale, scale, new Vector2(0, 0)) * Matrix3x2.Translation(_position);

            WindowRenderTarget target = _dx2d.RenderTarget;
            _oldMatrix = target.Transform;
            target.Transform = _matrix;

            Bitmap bitmap = _dx2d.Bitmaps[render.Sprite.SpriteId];
            target.DrawBitmap(bitmap, 1, BitmapInterpolationMode.Linear);
            target.Transform = _oldMatrix;
        }
    }
}
