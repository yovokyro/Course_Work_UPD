using System;
using System.Windows;
using System.Windows.Controls;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();

            try
            {
                if (Properties.Settings.Default.MusicVolume > 1)
                    musicVolume.Value = 100;
                else
                if (Properties.Settings.Default.MusicVolume < 0)
                    musicVolume.Value = 0;
                else
                    musicVolume.Value = Properties.Settings.Default.MusicVolume * 100;

                if (Properties.Settings.Default.SoundsVolume > 1)
                    soundsVolume.Value = 100;
                else
              if (Properties.Settings.Default.SoundsVolume < 0)
                    soundsVolume.Value = 0;
                else
                    soundsVolume.Value = Properties.Settings.Default.SoundsVolume * 100;
            }
            catch { }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MusicVolume = Convert.ToSingle(musicVolume.Value / 100);
            Properties.Settings.Default.SoundsVolume = Convert.ToSingle(soundsVolume.Value / 100);
        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
