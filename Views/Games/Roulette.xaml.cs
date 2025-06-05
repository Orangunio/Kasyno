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
using Kasyno.ViewModels;

namespace Kasyno.Views.Games
{
    /// <summary>
    /// Logika interakcji dla klasy Roulette.xaml
    /// </summary>
    public partial class Roulette : Window
    {
        public Roulette()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Bind command, or hook into SpinCommand if needed
        }

        private void SpinAnimation()
        {
            var rotateTransform = WheelRotation;
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 360 * 5, // obrót 5 razy
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (RouletteViewModel)this.DataContext;
            viewModel.Spin(2, 2); // uruchamia logikę
            SpinAnimation(); // animacja
        }
    }

}
