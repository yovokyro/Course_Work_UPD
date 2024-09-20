using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using Miner.Object;

namespace MinerTests.Players
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void PlayerUp()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            player.Up();
            Assert.AreEqual(95, player.Position.Y);
            Assert.AreEqual(100, player.Position.X);
        }

        [TestMethod]
        public void PlayerDown()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            player.Down();
            Assert.AreEqual(105, player.Position.Y);
            Assert.AreEqual(100, player.Position.X);
        }

        [TestMethod]
        public void PlayerLeft()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            player.Left();
            Assert.AreEqual(100, player.Position.Y);
            Assert.AreEqual(95, player.Position.X);
        }

        [TestMethod]
        public void PlayerRight()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            player.Right();
            Assert.AreEqual(100, player.Position.Y);
            Assert.AreEqual(105, player.Position.X);
        }

        [TestMethod]
        public void PlayerKill()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            player.Kill();
            Assert.IsFalse(player.Live);
        }

        [TestMethod]
        public void PlayerSpeedNormal()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            float speed = player.MovementSpeed;
            player.Speed(false);
            Assert.AreEqual(speed, player.MovementSpeed);
        }

        [TestMethod]
        public void PlayerSpeedSlowly()
        {
            Player player = new Player(new Vector2(100, 100), 5);
            float speed = player.MovementSpeed;
            player.Speed(true);
            Assert.AreEqual(speed - 1f, player.MovementSpeed);
        }
    }
}
