using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Kasyno.Models;
using Kasyno.ViewModels.Commands.RouletteCommands;

namespace Kasyno.ViewModels
{
    public class RouletteFieldDisplay
    {
        public string Label { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Brush Color { get; set; }
    }
    public class RouletteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int moneyToBet { get; set; } = 5;

        private int _currentBet;
        public int currentBet
        {
            get => _currentBet;
            set
            {
                if (_currentBet != value)
                {
                    _currentBet = value;
                    OnPropertyChanged();
                }
            }
        }


        public double CanvasSize => 300;
        public int userBalance => App.User.Balance;
        public ICommand SpinCommand => new SpinCommand(_ => Spin(2));
        public ObservableCollection<RouletteFieldDisplay> DisplayFields { get; set; } = new();

        public Roulette roulette = new Roulette();

        public ObservableCollection<IRouletteBetFields> betFields { get; set; } = new ObservableCollection<IRouletteBetFields>();

        public RouletteViewModel() { GenerateDisplayFields(); }
        public void GenerateDisplayFields()
        {
            DisplayFields.Clear();

            double width = CanvasSize;
            double height = CanvasSize;

            double centerX = width / 2;
            double centerY = height / 2;
            double fieldHeight = 30;
            double fieldWidth = 30;
            double radius = 180;

            int count = roulette.roulleteFields.Count;

            for (int i = 0; i < count; i++)
            {
                var field = roulette.roulleteFields[i];

                double angle = (2 * Math.PI * i) / count;
                double x = centerX + radius * Math.Cos(angle) - fieldWidth / 2;
                double y = centerY + radius * Math.Sin(angle) - fieldHeight / 2;

                Brush brush = field.color switch
                {
                    "red" => Brushes.Red,
                    "black" => Brushes.Black,
                    "green" => Brushes.Green,
                    _ => Brushes.Gray
                };

                string label = field.number == -1 ? "00" : field.number.ToString();

                DisplayFields.Add(new RouletteFieldDisplay
                {
                    Label = label,
                    X = x,
                    Y = y,
                    Color = brush
                });
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncreaseBet()
        {
            int increaseBetValue = userBalance * moneyToBet / 100;
            if (currentBet + increaseBetValue <= userBalance)
            {
                currentBet += increaseBetValue;
            }
            else
            {
                currentBet = userBalance;
            }
        }

        public void ReduceBet()
        {
            int reydecBetValue = userBalance * moneyToBet / 100;
            if (currentBet - reydecBetValue >= 0)
            {
                currentBet -= reydecBetValue;
            }
            else
            {
                currentBet = 0;
            }
        }

        public void Spin(int strength)
        {
            int spinPower = SpinPower(strength);
        }

        public int SpinPower(int strength = 1)
        {
            Random rnd = new Random();
            switch(strength)
            {
                case 1:
                    {
                        return rnd.Next(10, 20);
                    }
                case 2:
                    {
                        return rnd.Next(25, 30);
                    }
                case 3:
                    {
                        return rnd.Next(30, 40);
                    }
                case 4:
                    {
                        return rnd.Next(50, 70);
                    }
                default:
                    {
                        throw new Exception("Invalid Variable");
                    }
            }
        }

        public void CreateBetOnNumber(string color, int number)
        {
            if(currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }

            string convertedColor = ConvertColorCodeOnColor(color);

            NumberBet bet = new NumberBet(currentBet, number, convertedColor);
            betFields.Add(bet);
        }

        public string ConvertColorCodeOnColor(string colorCode)
        {
            if (Equals(colorCode, "#FFFF0000")) { return "Czerwone"; }
            else if (Equals(colorCode, "#FF000000")) { return "Czarne"; }
            else if (Equals(colorCode, "#FF008000")) { return "Zielone"; }
            return "????????";
        }
    }
}
