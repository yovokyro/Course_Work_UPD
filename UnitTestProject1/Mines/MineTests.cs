using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using Miner.Object;
using Miner.ObjectGraph;
using Miner.Object.MineSelection;

namespace MinerTests.Players
{
    [TestClass]
    public class MineTests
    {
        [TestMethod]
        public void MineTime()
        {
            MineGame mine = new MineGame(new LargeMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(3f, mine.Time);
        }

        [TestMethod]
        public void MineLargeRadius()
        {
            MineGame mine = new MineGame(new LargeMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(5, mine.Mine.Radius);
        }

        [TestMethod]
        public void MineMediumRadius()
        {
            MineGame mine = new MineGame(new MediumMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(4, mine.Mine.Radius);
        }

        [TestMethod]
        public void MineSmallRadius()
        {
            MineGame mine = new MineGame(new SmallMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(3, mine.Mine.Radius);
        }

        [TestMethod]
        public void MineSmallType()
        {
            MineGame mine = new MineGame(new SmallMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(Miner.Object.Enum.MineType.Small, mine.Mine.Type);
        }

        [TestMethod]
        public void MineMediumType()
        {
            MineGame mine = new MineGame(new MediumMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(Miner.Object.Enum.MineType.Medium, mine.Mine.Type);
        }

        [TestMethod]
        public void MineLargeType()
        {
            MineGame mine = new MineGame(new LargeMine(new Mine(new Vector2(100, 100))), new Sprite(0, 10, 10, 0), 3f, 1f);
            Assert.AreEqual(Miner.Object.Enum.MineType.Large, mine.Mine.Type);
        }
    }
}
