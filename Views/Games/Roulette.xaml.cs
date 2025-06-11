using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Kasyno.Models;
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
            int power = ((RouletteViewModel)this.DataContext).SpinPower((int)SpinPowerSlider.Value);
            int randomAngle = new Random().Next(70, 100);
            var rotateTransform = WheelRotation;
            int finalAngle = randomAngle * power;
            int animationTime = (int)AnimationLengthSlider.Value;
            double normalizedAngle = finalAngle % 360;

            if (animationTime <= 0)
            {
                rotateTransform.Angle = finalAngle % 360;
                WinningField(finalAngle);
                ButtonSpinner.IsEnabled = true;
                return;
            }

            var animation = new DoubleAnimation
            {
                From = 0,
                To = finalAngle,
                Duration = new Duration(TimeSpan.FromSeconds(animationTime)),
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
            };

            animation.Completed += (s, e) =>
            {
                try
                {
                    WinningField(finalAngle);
                    ButtonSpinner.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd: " + ex.Message);
                }
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private void WinningField(int finalAngle)
        {
            var viewModel = (RouletteViewModel)this.DataContext;
            viewModel.WinningField(finalAngle);
            ShowLastSevenWinningFields();
            LoadMoney();
            //MessageBox.Show($"Index: {index}\nFinalAngle: {finalAngle}\nLabel: {display.Label}\nColor: {display.Color}");
        }

        private void ShowLastSevenWinningFields()
        {
            LastWinsGrid.Children.Clear();
            var vm = (RouletteViewModel)this.DataContext;
            for (int i = 0; i < vm.WinningFields.Count && i < 7; i++)
            {
                RouletteField field = vm.WinningFields[i];
                string color = field.color;
                int number = field.number;

                Border cell = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1)
                };

                TextBlock text = new TextBlock
                {
                    Text = field.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };

                cell.Child = text;

                Grid.SetColumn(cell, i);

                LastWinsGrid.Children.Add(cell);
            }
        }

        private void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            if(((RouletteViewModel)this.DataContext).Spin())
            {
                SpinAnimation();
            }
        }

        private void CreateBetOnNumber(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            string color = button.Background.ToString();
            int number = int.Parse(button.Content.ToString());

            ((RouletteViewModel)this.DataContext).CreateBetOnNumber(color, number);
            ShowMoneyForBet();
        }

        private void CreateBetOnFirstTwelve(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnFirstTwelve();
            ShowMoneyForBet();
        }

        private void CreateBetOnSecondTwelve(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnSecondTwelve();
            ShowMoneyForBet();
        }

        private void CreateBetOnThirdTwelve(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnThirdTwelve();
            ShowMoneyForBet();
        }

        private void CreateBetOnFirstHalf(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnFirstHalf();
            ShowMoneyForBet();
        }

        private void CreateBetOnSecondHalf(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnSecondHalf();
            ShowMoneyForBet();
        }

        private void CreateBetOnEven(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnEven();
            ShowMoneyForBet();
        }

        private void CreateBetOnOdd(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnOdd();
            ShowMoneyForBet();
        }

        private void CreateBetOnRed(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnRed();
            ShowMoneyForBet();
        }

        private void CreateBetOnBlack(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnBlack();
            ShowMoneyForBet();
        }

        private void CreateBetOnFirstColumn(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnFirstColumn();
            ShowMoneyForBet();
        }

        private void CreateBetOnSecondColumn(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnSecondColumn();
            ShowMoneyForBet();
        }

        private void CreateBetOnThirdColumn(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).CreateBetOnThirdColumn();
            ShowMoneyForBet();
        }

        private void DeleteBet_Click(object sender, RoutedEventArgs e)
        {
            if (Bets.SelectedItem is IRouletteBetFields selectedBet)
            {
                ((RouletteViewModel)this.DataContext).DeleteBet(selectedBet);
                ShowMoneyForBet();
            }
        }

        private void ResetBets(object sender, RoutedEventArgs e)
        {
            ((RouletteViewModel)this.DataContext).ResetBets();
            ShowMoneyForBet();
        }

        private void ShowMoneyForBet()
        {
            string moneyForBet = ((RouletteViewModel)this.DataContext).moneyForBet.ToString();
            if (Equals(moneyForBet, "0"))
            {
                MoneyForBet.Text = "KOSZT ZAKŁADU";
            }
            else
            {
                MoneyForBet.Text = ((RouletteViewModel)this.DataContext).moneyForBet.ToString();
            }
        }
    }
}