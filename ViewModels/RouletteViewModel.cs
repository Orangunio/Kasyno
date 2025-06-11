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

        private int _moneyForBet = 0;
        public int moneyForBet 
        { 
            get => _moneyForBet; 
            set
            {
                if (_moneyForBet != value)
                {
                    _moneyForBet = value;
                    OnPropertyChanged();
                }
            }
        }

        public double CanvasSize => 300;
        public int userBalance => App.User.Balance;
        public ICommand SpinCommand => new SpinCommand(_ => Spin());
        public ObservableCollection<RouletteFieldDisplay> DisplayFields { get; set; } = new();
        public ObservableCollection<RouletteField> WinningFields { get; set; } = new();

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

        public bool Spin()
        {
            if(moneyForBet > App.User.Balance || moneyForBet == 0)
            {
                MessageBox.Show("Nie możesz stworzyć takiego zakładu");
                return false;
            }
            App.User.Balance -= moneyForBet;
            return true;
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

        public void WinningField(int finalAngle)
        {
            int displayFieldsCount = DisplayFields.Count;

            double anglePerField = 360.0 / displayFieldsCount;
            double normalizedAngle = finalAngle % 360;

            int index = (int)(((360 - normalizedAngle) % 360) / anglePerField) % displayFieldsCount;

            RouletteFieldDisplay display = DisplayFields[index];
            UpdateWinningFieldHistory(display);
        }

        public void UpdateWinningFieldHistory(RouletteFieldDisplay display)
        {
            int number;
            if (Equals(display.Label, "00"))
            {
                number = -1;
            }
            else
            {
                number = int.Parse(display.Label);
            }
            RouletteField field = new RouletteField(number, display.Color.ToString());
            WinningFields.Add(field);
            if (WinningFields.Count > 7)
            {
                WinningFields.RemoveAt(0);
            }
            CheckWinPrice(field);
        }

        public void CheckWinPrice(RouletteField winningField)
        {
            string convertedColor = ConvertColorCodeOnColor(winningField.color);
            winningField.color = convertedColor;
            int moneyWon = 0 - moneyForBet;
            foreach(var bet in betFields)
            {
                if (bet.checkWin(winningField))
                {
                    App.User.Balance += bet.potentialWin();
                    moneyWon += bet.potentialWin();
                }
            }
            string convertedColorOnCode = ConvertColorOnColorCode(winningField.color);
            winningField.color = convertedColorOnCode;
            //MessageBox.Show("Twój Bilans: " + moneyWon.ToString());
        }

        public void CreateBetOnNumber(string color, int number)
        {
            if(currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }

            moneyForBet += currentBet;
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

        public string ConvertColorOnColorCode(string color)
        {
            if (Equals(color, "Czerwone")) { return "#FFFF0000"; }
            else if (Equals(color, "Czarne")) { return "#FF000000"; }
            else if (Equals(color, "Zielone")) { return "#FF008000"; }
            return "????????";
        }

        public void CreateBetOnFirstTwelve()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            FirstTwelve bet = new FirstTwelve(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnSecondTwelve()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            SecondTwelve bet = new SecondTwelve(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnThirdTwelve()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            ThirdTwelve bet = new ThirdTwelve(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnFirstHalf()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            FirstHalf bet = new FirstHalf(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnSecondHalf()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            SecondHalf bet = new SecondHalf(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnEven()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            Even bet = new Even(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnOdd()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            Odd bet = new Odd(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnRed()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            Red bet = new Red(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnBlack()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            Black bet = new Black(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnFirstColumn()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            FirstColumn bet = new FirstColumn(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnSecondColumn()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            SecondColumn bet = new SecondColumn(currentBet);
            betFields.Add(bet);
        }

        public void CreateBetOnThirdColumn()
        {
            if (currentBet == 0)
            {
                MessageBox.Show("Nie możesz zagrać za 0");
                return;
            }
            moneyForBet += currentBet;
            ThirdColumn bet = new ThirdColumn(currentBet);
            betFields.Add(bet);
        }

        public void DeleteBet(IRouletteBetFields selectedBet)
        {
            moneyForBet -= selectedBet.bet;
            betFields.Remove(selectedBet);
        }

        public void ResetBets()
        {
            foreach(var bet in betFields.ToList())
            {
                betFields.Remove(bet);
            }
            moneyForBet = 0;
        }
    }
}
