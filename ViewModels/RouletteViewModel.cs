using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class RouletteViewModel
    {
        public double CanvasSize => 300;
        public ICommand SpinCommand => new SpinCommand(_ => Spin(2, 2));
        public ObservableCollection<RouletteFieldDisplay> DisplayFields { get; set; } = new();

        public Roulette roulette = new Roulette();

        public ObservableCollection<IRouletteBetFields> betFields = new ObservableCollection<IRouletteBetFields>();

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

        public void Spin(int strength, int animationType)
        {
            getBetFields();

            reducePlayerCash();

            int spinPower = SpinPower(strength, animationType);

            RouletteField winningField = getResult(spinPower);

            calculateProfit(winningField);

            updateUserCash();
        }

        public void getBetFields()
        {

        }

        public void reducePlayerCash()
        {
            int cash = 0;
            foreach(var betField in betFields)
            {
                cash += betField.bet;
            }

            //Tu logika pomniejszenia gotowki gracza
        }

        //AnimationFast:
        //1 - 2 razy wolniej
        //2 - normalnie
        //3 - 2 razy szybciej
        //4 - display wynik


        //Strength
        //1 - 20-40
        //2 - 35-55
        //3 - 50-80
        //4 - 70-100

        public int SpinPower(int strength, int animationType)
        {
            Random rnd = new Random();
            switch(strength)
            {
                case 1:
                    {
                        return rnd.Next(20, 40);
                    }
                case 2:
                    {
                        return rnd.Next(35, 55);
                    }
                case 3:
                    {
                        return rnd.Next(50, 80);
                    }
                case 4:
                    {
                        return rnd.Next(70, 100);
                    }
                default:
                    {
                        throw new Exception("Invalid Variable");
                    }
            }
        }

        public RouletteField getResult(int spinPower)
        {
            int winingIndex = spinPower % roulette.roulleteFields.Count();
            return roulette.roulleteFields[winingIndex];
        }

        public int calculateProfit(RouletteField winningField)
        {
            int profit = 0;
            foreach(var betField in betFields)
            {
                if(betField.checkWin(winningField))
                {
                    profit += betField.potentialWin();
                }
            }
            return profit;
        }

        public void updateUserCash()
        {

        }
    }
}
