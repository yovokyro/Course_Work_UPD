using SharpDX;
using Miner.Object;

namespace Miner.ObjectGraph
{
    /// <summary>
    /// Абстрактный класс для рендера объектов. Определяет их rectangle, размеры и отображаемый спрайт
    /// </summary>
    public abstract class RenderObject
    {
        protected ObjectGame objectGame;
        //объект класса ObjectGame
        public ObjectGame ObjectGame { get => objectGame; }

        protected Sprite sprite;
        //объект класса Sprite
        public Sprite Sprite { get => sprite; }
        //объекта типа RectangleF
        public RectangleF Rect { get => GetRect(); }
     
        protected float scale;
        //размер спрайта
        public float Scale { get => scale; }
        public RenderObject(ObjectGame objectGame, Sprite sprite)
        {
            this.objectGame = objectGame;
            this.sprite = sprite;
            scale = 1f;
        }
        /// <summary>
        /// Определяет RectangleF относительно спрайта и позиции игрового объекта
        /// </summary>
        /// <returns>Возвражает RectangleF</returns>
        private RectangleF GetRect()
        {
            return new RectangleF(objectGame.Position.X - sprite.Center.X, objectGame.Position.Y - sprite.Center.Y, sprite.Width, sprite.Heigth);
        } 

    }
}
