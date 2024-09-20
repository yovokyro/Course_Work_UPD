using Microsoft.VisualStudio.TestTools.UnitTesting;
using Miner;
using Miner.Object.Enum;
using Miner.ObjectGraph;

namespace MinerTests.Walls
{
    [TestClass]
    public class WallTests
    {
        [TestMethod]
        public void WallWeakTest()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallWeak(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();

            Assert.AreEqual(WallType.Weak, wallGame.Wall.Type);
            Assert.IsTrue(wallGame.Wall.Destructibility);
            Assert.AreEqual(1, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallMediumTest()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallMedium(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();

            Assert.AreEqual(WallType.Medium, wallGame.Wall.Type);
            Assert.IsTrue(wallGame.Wall.Destructibility);
            Assert.AreEqual(3, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallEndlessTest()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallEndless(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();

            Assert.AreEqual(WallType.Endless, wallGame.Wall.Type);
            Assert.IsFalse(wallGame.Wall.Destructibility);
            Assert.AreEqual(1, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallWeakDamage()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallWeak(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();
            wallGame.Wall.Damage();

            Assert.AreEqual(0, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallMediumDamage()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallMedium(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();
            wallGame.Wall.Damage();

            Assert.AreEqual(2, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallEndlessDamage()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallEndless(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();
            wallGame.Wall.Damage();

            Assert.AreEqual(1, wallGame.Wall.Health);
        }

        [TestMethod]
        public void WallLiveTrue()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallWeak(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();
            Assert.IsTrue(wallGame.Wall.GetLive());
        }

        [TestMethod]
        public void WallLiveFalse()
        {
            WallGame wallGame;
            Creator creator;
            Wall wall = new WallWeak(10, 10, 1, 0);
            creator = wall.Create();
            wallGame = creator.CreateMap();
            wallGame.Wall.Damage();

            Assert.IsFalse(wallGame.Wall.GetLive());
        }
    }
}
