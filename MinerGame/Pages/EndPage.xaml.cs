using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using UPDController;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для EndPage.xaml
    /// </summary>
    public partial class EndPage : Page
    {
        private ISocket _socket;
        public EndPage(string winText, ISocket socket)
        {
            InitializeComponent();
            output.Content = winText;
            _socket = socket;
        }

        private void mainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_socket != null)
            {
                _socket.ClearInstance();
                _socket = null;
            }
        }
    }
}
