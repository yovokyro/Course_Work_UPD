using MinerGame.UPD;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            Server server = new Server();
            NavigationService.Navigate(new ComplexityPage());
        }

        private void buttonJoin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComplexityPage());
        }

        private void buttonSettingOpen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingPage()); 
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
           Application.Current.MainWindow.Close();
        }
    }
}
