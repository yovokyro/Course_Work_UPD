using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UPDController;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadingServer.xaml
    /// </summary>
    public partial class LoadingServer : Page
    {
        private Client _socket;

        public LoadingServer(ISocket socket)
        {
            InitializeComponent();
            _socket = (Client)socket;
            yourIP.Content = $"CLIENT IP: {_socket.GetAddress()}";
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            textError.Visibility = Visibility.Hidden;

            string text = ip.Text.Replace(" ", "");
            string[] split = text.Split(':');

            if (split.Length == 2)
            {
                string ipAddress = split[0];
                string portStr = split[1];

                if (int.TryParse(portStr, out int port))
                {
                    if (port >= 0 && port <= 65535)
                    {
                        string ping = $"ping {_socket.Port}";
                        _socket.Connection(ipAddress, port, ping);
                        await Task.Delay(100);

                        if(_socket.IsConnection)
                        {
                            NavigationService.Navigate(new ShopPage(_socket, _socket.Money));
                        }
                        else
                        {
                            ErrorOutput("Не удалось подключиться.");
                        }
                    }
                    else
                    {
                        ErrorOutput("Неверный порт. Порт должен быть в диапазоне от 0 до 65535.");
                    }
                }
                else
                {
                    ErrorOutput("Неверный формат порта.");
                }
            }
            else
            {
                ErrorOutput("Неверный формат IP:порт.");
            }
        }

        private void ErrorOutput(string error)
        {
            textError.Visibility = Visibility.Visible;
            textError.Content = error;
        }
    }
}
