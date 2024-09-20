using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System.Collections.Generic;

namespace MinerTests.Players
{
    [TestClass]
    public class PlayerGameTests
    {
        [TestMethod]
        public void PlayerGameMiningBefore()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(1, 10, 10, 0), mines);
            Assert.IsTrue(player.GetMining() && player.Installation);
        }

        [TestMethod]
        public void PlayerGameMiningAfter()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(0, 10, 10, 0), mines);

            player.GetMining();
            player.GetMining();

            Assert.IsFalse(player.GetMining());
            Assert.IsTrue(player.Installation);
        }

        [TestMethod]
        public void PlayerGameBoom()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(0, 10, 10, 0), mines);
            player.GetMining();
            player.Boom();

            Assert.IsFalse(player.Installation);
        }

        [TestMethod]
        public void PlayerGameLive()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(0, 10, 10, 0), mines);
            Assert.IsTrue(player.GetLive());
        }

        [TestMethod]
        public void PlayerGameKill()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(0, 10, 10, 0), mines);
            player.Kill();

            Assert.IsFalse(player.GetLive());
        }

        [TestMethod]
        public void PlayerGameGetMineCheck()
        {
            Dictionary<int, int> mines = new Dictionary<int, int>
            {
                { 1, 1 },
                { 2, 0 },
                { 3, 0 }
            };

            Miner.ObjectGraph.PlayerGame player = new Miner.ObjectGraph.PlayerGame(new Miner.Object.Player(new Vector2(100, 100), 5), new Miner.Object.Sprite(0, 10, 10, 0), mines);
            Assert.IsTrue(player.GetChooseMina(1));
            Assert.IsFalse(player.GetChooseMina(2));
            Assert.IsFalse(player.GetChooseMina(3));
        }
    }
}
