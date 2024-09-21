﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using UPDController;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        private PlayerSetting player;

        private ISocket _socket;

        private Color backgroundNoDone = Color.FromArgb(0x66, 0xFF, 0x7F, 0x00);
        private Color backgroundDone = Color.FromArgb(0x66, 0x00, 0xFF, 0x00);

        private Color borderNoDone = Color.FromArgb(0xFF, 0xFF, 0x7F, 0x00);
        private Color borderDone = Color.FromArgb(0xFF, 0x08, 0xFF, 0x00);

        private bool IsDone = false;

        private float _musicVolume;
        private float _soundsVolume;

        public ShopPage(ISocket socket, double money)
        {
            InitializeComponent();

            _socket = socket;
            if (_socket.Type == SocketTypes.Server)
            {
                if (_socket is Server serverSocket)
                {
                    serverSocket.SetMoney(money);
                }

                buttonDone.Visibility = Visibility.Hidden;
                buttonStart.Visibility = Visibility.Visible;

                ip.Content = $"SERVER IP:\n{_socket.GetAddress()}";
            }
            else
            {
                playerName.Foreground = Brushes.Red;
                playerName.Text = "Игрок2";

                buttonDone.Visibility = Visibility.Visible;
                buttonStart.Visibility = Visibility.Hidden;

                ip.Content = $"CLIENT IP:\n{_socket.GetAddress()}";
            }


            player = new PlayerSetting(money);
            playerMoney.Content = player.Money;

            _musicVolume = Properties.Settings.Default.MusicVolume;
            _soundsVolume = Properties.Settings.Default.SoundsVolume;

            BlockButton();
        }

        private void buttonLowMine_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 10)
            {
                player.Buy(0, 10);
                playerMoney.Content = player.Money;
                lowCount.Content = player.Mines[0];

                BlockButton();
            }
        }

        private void buttonMediumMine_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 20)
            {
                player.Buy(1, 20);
                playerMoney.Content = player.Money;
                mediumCount.Content = player.Mines[1];

                BlockButton();
            }
        }

        private void buttonHardMine_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 50)
            {
                player.Buy(2, 50);
                playerMoney.Content = player.Money;
                highCount.Content = player.Mines[2];

                BlockButton();
            }
        }


        private void BlockButton()
        {
            if (player.Money < 50 && buttonLowMine.IsEnabled != false)
            {
                buttonHardMine.IsEnabled = false;

                if (player.Money < 20)
                {
                    buttonMediumMine.IsEnabled = false;

                    if (player.Money < 10)
                    {
                        buttonLowMine.IsEnabled = false;
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
                if (player.MineCount != 0)
                {
                    string name = playerName.Text.Replace(" ", "");

                    if (name.Length != 0)
                    {
                        errorOutput.Visibility = Visibility.Hidden;

                        if (_socket is Server serverSocket)
                        {
                            if(!serverSocket.ClientReady)
                            {
                                errorOutput.Visibility = Visibility.Visible;
                                errorOutput.Content = "Клиент не готов.";
                                return;
                            }
                        }

                        player.SetName(name);

                        /* Application.Current.MainWindow.IsEnabled = false;
                         Application.Current.MainWindow.Visibility = Visibility.Collapsed;
                         Miner.Application game;

                             game = new Miner.Application(_mineCountPlayer1, _mineCountPlayer2, _name, _musicVolume, _soundsVolume);
                             game.Run();
                             game.Dispose();

                             NavigationService.Navigate(new EndPage(game.Win));


                         Application.Current.MainWindow.IsEnabled = true;
                         Application.Current.MainWindow.Visibility = Visibility.Visible;*/
                    }

                    else
                    {
                        errorOutput.Visibility = Visibility.Visible;
                        errorOutput.Content = "Имя не может состоять из пробелов или быть пустым.";
                    }
                }
                else
                {
                    errorOutput.Visibility = Visibility.Visible;
                    errorOutput.Content = "Купите мины для старта.";
                }
            }
            catch { }
        }

        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (player.MineCount != 0)
                {
                    string name = playerName.Text.Replace(" ", "");

                    if (name.Length != 0)
                    {
                        errorOutput.Visibility = Visibility.Hidden;

                        IsDone = !IsDone;
                        SolidColorBrush backgroundBrush;
                        SolidColorBrush borderBrush;
                        if (IsDone)
                        {
                            player.SetName(name);

                            backgroundBrush = new SolidColorBrush(backgroundDone);
                            borderBrush = new SolidColorBrush(borderDone);
                            buttonDone.Background = backgroundBrush;
                            buttonDone.BorderBrush = borderBrush;
                            buttonDone.Content = "Готов";


                            buttonBack.IsEnabled = false;
                        }
                        else
                        {
                            backgroundBrush = new SolidColorBrush(backgroundNoDone);
                            borderBrush = new SolidColorBrush(borderNoDone);
                            buttonDone.Background = backgroundBrush;
                            buttonDone.BorderBrush = borderBrush;
                            buttonDone.Content = "Не готов";

                            buttonBack.IsEnabled = true;
                        }

                        if (_socket is Client clientSocket)
                        {
                            string message = $"ClientReady {IsDone}";
                            clientSocket.SendMessageAsync(message);
                        }
                    }

                    else
                    {
                        errorOutput.Visibility = Visibility.Visible;
                        errorOutput.Content = "Имя не может состоять из пробелов или быть пустым.";
                    }
                }
                else
                {
                    errorOutput.Visibility = Visibility.Visible;
                    errorOutput.Content = "Купите мины для готовности.";
                }
            }
            catch { }
        }
    }
}
