using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MinerGame.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        private string pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

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

                ip.Text = Properties.Settings.Default.IP;
                port.Text = Properties.Settings.Default.PORT;
                freePortCheck.IsChecked = Properties.Settings.Default.FreePort;

                if(Properties.Settings.Default.FreePort)
                {
                    port.IsEnabled = false;
                }

            }
            catch { }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            error.Visibility = Visibility.Hidden;

            string ipAddress = ip.Text;
            string portStr = port.Text;

            if (!Regex.IsMatch(ipAddress, pattern))
            {
                ErrorOutput("Неверный формат IP.");
                return;
            }


            if (!freePortCheck.IsChecked.Value)
            {
                if (int.TryParse(portStr, out int portCheck))
                {
                    if (portCheck < 0 || portCheck > 65535)
                    {
                        ErrorOutput("Неверный порт. Порт должен быть в диапазоне от 0 до 65535;");
                        return;
                    }  
                }
                else
                {
                    ErrorOutput("Неверный формат порта.");
                    return;
                }
            }
            else
            {
                portStr = "0";
            }

            Properties.Settings.Default.MusicVolume = Convert.ToSingle(musicVolume.Value / 100);
            Properties.Settings.Default.SoundsVolume = Convert.ToSingle(soundsVolume.Value / 100);

            Properties.Settings.Default.IP = ipAddress;
            Properties.Settings.Default.PORT = portStr;
            Properties.Settings.Default.FreePort = freePortCheck.IsChecked.Value;

            NavigationService.GoBack();

        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void freePortCheck_Click(object sender, RoutedEventArgs e)
        {
            port.IsEnabled = !freePortCheck.IsChecked.Value;
        }

        private void ErrorOutput(string message)
        {
            error.Content = message;
            error.Visibility = Visibility.Visible;
        }
    }
}
