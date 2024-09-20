using SharpDX;
using Miner.ObjectGraph;
using Miner.Object;
using Miner.Object.Enum;

namespace Miner
{ 
    /// <summary>
    /// Абстрактный класс Wall
    /// </summary>
    public abstract class Wall 
    {
        protected float height;
        protected float width;

        protected int size;

        protected int index;

        public Wall(float height, float width, int size, int index) 
        { 
            this.height = height;
            this.width = width;
            this.size = size;
            this.index = index;
        }

        /// <summary>
        /// Абстрактный метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Create</returns>
        abstract public Creator Create();
    }

    /// <summary>
    /// Класс, описывающий слабую стену
    /// </summary>
    public class WallWeak : Wall
    {
        public WallWeak(float height, float width, int size, int index) : base(height, width, size, index) 
        { 

        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Create</returns>
        public override Creator Create()
        {
            return new WallWeakCreate(height, width, size, index);
        }
    }

    /// <summary>
    /// Класс, описывающий среднюю стену
    /// </summary>
    public class WallMedium : Wall
    {
        public WallMedium(float height, float width, int size, int index) : base(height, width, size, index) 
        {

        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Create</returns>
        public override Creator Create()
        {
            return new WallMediumCreate(height, width, size, index);
        }
    }

    /// <summary>
    /// Класс, описывающий сильную стену
    /// </summary>
    public class WallEndless : Wall
    {
        public WallEndless(float height, float width, int size, int index) : base(height, width, size, index)
        {

        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Create</returns>
        public override Creator Create()
        {
            return new WallEndlessCreate(height, width, size, index);
        }
    }

    /// <summary>
    /// Абстрактный класс Create
    /// </summary>
    public abstract class Creator
    {
        protected WallGame wg;
        protected bool destructibility;
        protected int health;

        /// <summary>
        /// Абстрактный метод
        /// </summary>
        /// <returns>Возвращает объекта класса WallGame</returns>
        public abstract WallGame CreateMap();
    }

    /// <summary>
    /// Класс, описывающий слабую стену
    /// </summary>
    public class WallWeakCreate : Creator
    {
        public WallWeakCreate(float height, float width, int size, int index)
        {
            destructibility = true;
            health = 1;
            wg = new WallGame(new Object.Wall(new Vector2(height, width), health, destructibility, WallType.Weak), new Sprite(index, size, size, 0));
        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает объекта класса WallGame</returns>
        public override WallGame CreateMap()
        {
            return wg;
        }
    }

    /// <summary>
    /// Класс, описывающий среднюю стену
    /// </summary>
    public class WallMediumCreate : Creator
    {
        public WallMediumCreate(float height, float width, int size, int index)
        {
            destructibility = true;
            health = 3;
            wg = new WallGame(new Object.Wall(new Vector2(height, width), health, destructibility, WallType.Medium), new Sprite(index, size, size, 0));
        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает объекта класса WallGame</returns>
        public override WallGame CreateMap()
        {
            return wg;
        }
    }

    /// <summary>
    /// Класс, описывающий сильную стену
    /// </summary>
    public class WallEndlessCreate : Creator
    {
        public WallEndlessCreate(float height, float width, int size, int index)
        {
            destructibility = false;
            health = 1;
            wg = new WallGame(new Object.Wall(new Vector2(height, width), health, destructibility, WallType.Endless), new Sprite(index, size, size, 0));
        }

        /// <summary>
        /// Переопределенный метод
        /// </summary>
        /// <returns>Возвращает объекта класса WallGame</returns>
        public override WallGame CreateMap()
        {
            return wg;
        }
    }
}
