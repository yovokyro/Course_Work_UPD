using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UPDController;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComplexityPage.xaml
    /// </summary>
    public partial class ComplexityPage : Page
    {
        private double _money; //доступные деньги для игроков
        private ISocket _socket;

        public ComplexityPage()
        {
            InitializeComponent();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void buttonFastGame_Click(object sender, RoutedEventArgs e)
        {
            _money = 100;
            ViewShopPage();
        }

        private void buttonNormalGame_Click(object sender, RoutedEventArgs e)
        {
            _money = 250;
            ViewShopPage();
        }

        private void buttonLongGame_Click(object sender, RoutedEventArgs e)
        {
            _money = 500;
            ViewShopPage();
        }

        private void ViewShopPage()
        {
            string ipAddress = Properties.Settings.Default.IP;
            string port = Properties.Settings.Default.PORT;
            bool freePort = Properties.Settings.Default.FreePort;

            _socket = int.TryParse(port, out int PORT) ? Server.GetInstance(ipAddress, PORT, freePort) : Server.GetInstance();

            Console.WriteLine(_socket.GetInfo());

            NavigationService.Navigate(new ShopPage(_socket, _money));
        }

        private void ComplexityPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_socket != null)
            {
                _socket.ClearInstance();
                _socket = null;
            }
        }
    }
}
