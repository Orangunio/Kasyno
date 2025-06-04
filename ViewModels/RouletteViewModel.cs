using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kasyno.Models;

namespace Kasyno.ViewModels
{
    public class RouletteViewModel
    {
        public Roulette roulette = new Roulette();

        public ObservableCollection<IRouletteBetFields> betFields = new ObservableCollection<IRouletteBetFields>();

        public RouletteViewModel() { }

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
