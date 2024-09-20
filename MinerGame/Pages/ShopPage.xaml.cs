using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        private string[] _name;
        private double[] _money;

        private int[] _mineCount;

        private Dictionary<int, int> _mineCountPlayer1;
        private Dictionary<int, int> _mineCountPlayer2;

        private float _musicVolume;
        private float _soundsVolume;

        public ShopPage(double money)
        {
            InitializeComponent();
            _money = new double[2];
            _name = new string[2];
            _mineCount = new int[2];
            _mineCountPlayer1 = new Dictionary<int, int>();
            _mineCountPlayer2 = new Dictionary<int, int>();

            _money[0] = money;
            _money[1] = money;

            playerMoney1.Content = _money[0];
            playerMoney2.Content = _money[1];

            _mineCountPlayer1.Add(1, 0);
            _mineCountPlayer1.Add(2, 0);
            _mineCountPlayer1.Add(3, 0);

            _mineCountPlayer2.Add(1, 0);
            _mineCountPlayer2.Add(2, 0);
            _mineCountPlayer2.Add(3, 0);

            _musicVolume = Properties.Settings.Default.MusicVolume;
            _soundsVolume = Properties.Settings.Default.SoundsVolume;

            BlockButton();
        }

        private void buttonLowMine1_Click(object sender, RoutedEventArgs e)
        {
            if (_money[0] >= 10)
            {
                _mineCount[0]++;
                _money[0] -= 10;
                playerMoney1.Content = _money[0];
                lowCount1.Content = ++_mineCountPlayer1[1];

                BlockButton();
            }
        }

        private void buttonMediumMine1_Click(object sender, RoutedEventArgs e)
        {
            if (_money[0] >= 20)
            {
                _mineCount[0]++;
                _money[0] -= 20;
                playerMoney1.Content = _money[0];

                mediumCount1.Content = ++_mineCountPlayer1[2];

                BlockButton();
            }
        }

        private void buttonHardMine1_Click(object sender, RoutedEventArgs e)
        {
            if (_money[0] >= 50)
            {
                _mineCount[0]++;
                _money[0] -= 50;
                playerMoney1.Content = _money[0];

                highCount1.Content = ++_mineCountPlayer1[3];

                BlockButton();
            }
        }

        private void buttonLowMine2_Click(object sender, RoutedEventArgs e)
        {
            if (_money[1] >= 10)
            { 
                _mineCount[1]++;
                _money[1] -= 10;
                playerMoney2.Content = _money[1];

                lowCount2.Content = ++_mineCountPlayer2[1]; ;

                BlockButton();
            }
        }

        private void buttonMediumMine2_Click(object sender, RoutedEventArgs e)
        {
            if (_money[1] >= 20)
            {
                _mineCount[1]++;
                _money[1] -= 20;
                playerMoney2.Content = _money[1];

                mediumCount2.Content = ++_mineCountPlayer2[2];

                BlockButton();
            }
        }

        private void buttonHardMine2_Click(object sender, RoutedEventArgs e)
        {
            if (_money[1] >= 50)
            {
                _mineCount[1]++;
                _money[1] -= 50;
                playerMoney2.Content = _money[1];

                highCount2.Content = ++_mineCountPlayer2[3];

                BlockButton();
            }
        }

        private void BlockButton()
        {
            if (_money[0] < 50 && buttonLowMine1.IsEnabled != false)
            {
                buttonHardMine1.IsEnabled = false;

                if (_money[0] < 20)
                {
                    buttonMediumMine1.IsEnabled = false;

                    if (_money[0] < 10)
                    {
                        buttonLowMine1.IsEnabled = false;
                    }
                }
            }

            if (_money[1] < 50 && buttonLowMine2.IsEnabled != false)
            {
                buttonHardMine2.IsEnabled = false;

                if (_money[1] < 20)
                {
                    buttonMediumMine2.IsEnabled = false;

                    if (_money[1] < 10)
                    {
                        buttonLowMine2.IsEnabled = false;
                    }
                }
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mineCount[0] != 0 && _mineCount[1] != 0)
                {
                    _name[0] = playerName1.Text.Replace(" ", "");
                    _name[1] = playerName2.Text.Replace(" ", "");

                    if (_name[0].Length != 0 && _name[1].Length != 0)
                    {
                        Application.Current.MainWindow.IsEnabled = false;
                        Application.Current.MainWindow.Visibility = Visibility.Collapsed;
                        Miner.Application game;
                        
                            game = new Miner.Application(_mineCountPlayer1, _mineCountPlayer2, _name, _musicVolume, _soundsVolume);
                            game.Run();
                            game.Dispose();

                            NavigationService.Navigate(new EndPage(game.Win));
                    

                        Application.Current.MainWindow.IsEnabled = true;
                        Application.Current.MainWindow.Visibility = Visibility.Visible;
                    }

                    else
                    {
                        errorOutput.Visibility = Visibility.Visible;
                        errorOutput.Content = "Ошибка!\nИмя игрока не может\nсостоять из пробелов\nили быть пустым";
                    }
                }
                else
                {
                    errorOutput.Visibility = Visibility.Visible;
                    errorOutput.Content = "Ошибка!\nУ игрока нет мин";
                }
            }
            catch { }
        }
    }
}
