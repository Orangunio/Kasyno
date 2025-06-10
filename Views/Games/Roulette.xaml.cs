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
            LoadMoney();
            this.Closing += Roulette_Closing;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void LoadMoney()
        {
            try
            {
                UserMoney.Text = App.User.Balance.ToString("C2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania danych użytkownika: " + ex.Message);
                this.Close();
            }
        }
        private void IncreaseBet(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).IncreaseBet();
            MoneyValue.Text = ((RouletteViewModel)this.DataContext).currentBet.ToString();
        }

        private void ReduceBet(object render, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).ReduceBet();
            MoneyValue.Text = ((RouletteViewModel)this.DataContext).currentBet.ToString();
        }

        private void ChangeMoneyToBet(object render, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).moneyToBet = (int)MoneyBetPerClick.Value;
        }

        private void Roulette_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainMenu = new MainMenuView();
            mainMenu.Show();
        }

        private void SpinAnimation()
        {
            ButtonSpinner.IsEnabled = false;
            var viewModel = (RouletteViewModel)this.DataContext;
            int power = viewModel.SpinPower((int)SpinPowerSlider.Value);
            int randomAngle = new Random().Next(70, 90);
            int animationTime = (int)AnimationLengthSlider.Value;
            if (animationTime <= 0)
            {
                ButtonSpinner.IsEnabled = true;
            }
            var rotateTransform = WheelRotation;
            var animation = new DoubleAnimation
            {
                From = 0,
                To = randomAngle * power,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
            };

            animation.Completed += (s, e) =>
            {
                try
                {
                    ButtonSpinner.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd: " + ex.Message);
                }
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (RouletteViewModel)this.DataContext;
            viewModel.Spin(2);
            SpinAnimation();
        }

        private void CreateBetOnNumber(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            string color = button.Background.ToString();
            int number = int.Parse(button.Content.ToString());

            ((RouletteViewModel)this.DataContext).CreateBetOnNumber(color, number);
        }
    }
}
