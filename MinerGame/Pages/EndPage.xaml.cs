using System.Windows.Controls;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для EndPage.xaml
    /// </summary>
    public partial class EndPage : Page
    {
        public EndPage(string winText)
        {
            InitializeComponent();
            output.Content = winText;
        }

        private void mainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void moreGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComplexityPage());
        }
    }
}
