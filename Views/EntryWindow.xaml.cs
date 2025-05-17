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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kasyno.Views
{
    /// <summary>
    /// Logika interakcji dla klasy EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public EntryWindow()
        {
            InitializeComponent();
            var wejscie = (Storyboard)this.Resources["Wejscie"];
            wejscie?.Begin();

            var bouncing = (Storyboard)this.Resources["BouncingLetters"];
            bouncing?.Begin();
        }
        private async void RegisterTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await AnimatePanelSwitch(LoginPanel, RegisterPanel);
        }

        private async void LoginTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await AnimatePanelSwitch(RegisterPanel, LoginPanel);
        }
        private async Task AnimatePanelSwitch(StackPanel hidePanel, StackPanel showPanel)
        {
            // Pobierz storyboardy
            var fadeOut = (Storyboard)FindResource("FadeOut");
            var fadeIn = (Storyboard)FindResource("FadeIn");

            // Ustaw target animacji
            Storyboard.SetTarget(fadeOut, hidePanel);
            Storyboard.SetTarget(fadeIn, showPanel);

            // Animuj zaniknięcie
            fadeOut.Begin();
            await Task.Delay(300); // czekaj na animację

            // Ukryj i wyzeruj opacity
            hidePanel.Visibility = Visibility.Collapsed;
            hidePanel.Opacity = 1; // reset na wypadek ponownego pokazania

            // Pokaż i animuj pojawienie
            showPanel.Visibility = Visibility.Visible;
            showPanel.Opacity = 0;
            fadeIn.Begin();
            await Task.Delay(300);
        }

    }
}
