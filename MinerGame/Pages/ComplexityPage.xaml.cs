using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComplexityPage.xaml
    /// </summary>
    public partial class ComplexityPage : Page
    {
        private double _money; //доступные деньги для игроков
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

        private void ViewShopPage() //открытие след страницы
        {
            NavigationService.Navigate(new ShopPage(_money));
        }
    }
}
