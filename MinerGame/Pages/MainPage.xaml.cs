using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UPDController;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private ISocket _socket;

        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComplexityPage());
        }

        private void buttonJoin_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = Properties.Settings.Default.IP;
            string port = Properties.Settings.Default.PORT;
            bool freePort = Properties.Settings.Default.FreePort;

            _socket = int.TryParse(port, out int PORT) ? Client.GetInstance(ipAddress, PORT, freePort) : Client.GetInstance();
            Console.WriteLine(_socket.GetInfo());

            NavigationService.Navigate(new LoadingServer(_socket));
        }

        private void buttonSettingOpen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingPage()); 
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
           Application.Current.MainWindow.Close();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_socket != null)
            {
                _socket.StopReceive();
                _socket.ClearInstance();
                _socket = null;
            }
        }
    }
}
