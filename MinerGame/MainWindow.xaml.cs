using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MinerGame.Pages;

namespace MinerGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Icon = BitmapFrame.Create(new Uri(@"..\\..\\..\\Resources\\icon.ico", UriKind.RelativeOrAbsolute));
            }
            catch { }

            try
            {
                ImageBrush imgBrush = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(@"..\\..\\..\\Resources\\background.jpg", UriKind.RelativeOrAbsolute))
                };

                Background = imgBrush;
            }
            catch { }

            MainFrame.Content = new MainPage();
        }
    }
}
