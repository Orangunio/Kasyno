using Kasyno.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kasyno.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : Window
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RouletteButtonClick(object sender, RoutedEventArgs e)
        {
            var rouletteView = new Views.Games.Roulette();
            rouletteView.Show();
            Close();
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && combo.SelectedItem is ComboBoxItem item && item.Tag is string lang)
            {
                Properties.Settings.Default.AppCulture = lang;
                Properties.Settings.Default.Save();
                LocalizationManager.ChangeLanguage(lang);
            }
        }
    }
}
