using System.Collections.Generic;

namespace UPDController
{
    public class PlayerSetting
    {
        private string _name;
        public string Name => _name;

        private int _mineCount;
        public int MineCount => _mineCount;

        private double _money;

        public double Money => _money;

        private Dictionary<int, int> _mines;
        public Dictionary<int, int> Mines => _mines;

        public PlayerSetting(double money)
        {
            _name = "Player";
            _mineCount = 0;
            _mines = new Dictionary<int, int>();
            for (int i = 0; i < 3; i++)
            {
                _mines.Add(i, 0);
            }
            _money = money;
        }


        public void Buy(int mine, double money)
        {
            _mineCount++;
            _mines[mine]++;
            _money -= money;
        }

        public void SetName(string name) => _name = name;

    }
}
