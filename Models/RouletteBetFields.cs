using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kasyno.Models
{
    public interface IRouletteBetFields
    {
        public int bet {  get; set; }
        public bool checkWin(RouletteField winningField);
        public int potentialWin();
    }

    public class NumberBet: IRouletteBetFields
    {
        public int bet { get; set; }
        public int number {  get; set; }
        public string color { get; set; }
        public NumberBet(int bet, int number, string color)
        {
            this.bet = bet; this.number = number; this.color = color;
        }

        public bool checkWin(RouletteField winningField) 
        { 
            if(winningField.number == this.number && Equals(winningField.color,this.color))
            {
                return true;
            }
            return false;
        }

        public int potentialWin()
        {
            return this.bet * 36;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return color+" "+number+" Grasz za: "+bet+" Potencjalna Wygrana: "+potentialWin;
        }
    }

    public class FirstTwelve: IRouletteBetFields
    {
        public int bet { get; set; }
        public FirstTwelve(int bet)
        {
            this.bet = bet;
        }
        public bool checkWin(RouletteField winningField)
        {
            if(winningField.number <= 12 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }
        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return  "1st12 Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class SecondTwelve : IRouletteBetFields
    {
        public int bet { get; set; }
        public SecondTwelve(int bet)
        {
            this.bet = bet;
        }
        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number > 12 && winningField.number <= 24 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "2nd12 Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class ThirdTwelve : IRouletteBetFields
    {
        public int bet { get; set; }
        public ThirdTwelve(int bet)
        {
            this.bet = bet;
        }
        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number > 24 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "3rd12 Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class FirstHalf : IRouletteBetFields
    {
        public int bet { get; set; }
        public FirstHalf(int bet) 
        { 
            this.bet = bet;
        }

        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number <= 18 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "FirstHalf Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class SecondHalf : IRouletteBetFields
    {
        public int bet { get; set; }
        public SecondHalf(int bet) 
        { 
            this.bet = bet;
        }

        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number > 18 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "SecondHalf Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class Even : IRouletteBetFields
    {
        public int bet { get; set; }
        public Even(int bet)
        { 
            this.bet = bet;
        }

        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number % 2 == 0 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "Even Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class Odd : IRouletteBetFields
    {
        public int bet { get; set; }
        public Odd(int bet) { this.bet = bet; }

        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number % 2 != 0 && winningField.number != 0 && winningField.number != -1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "Odd Grasz za: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class Red : IRouletteBetFields
    {
        public int bet { get; set; }
        public Red(int bet) { this.bet = bet; }

        public bool checkWin(RouletteField winningField)
        {
            if (Equals(winningField.color, "red"))
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "Czerwone: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class Black : IRouletteBetFields
    {
        public int bet { get; set; }
        public Black(int bet) { this.bet = bet; }

        public bool checkWin(RouletteField winningField)
        {
            if (Equals(winningField.color, "black"))
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 2;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "Czarne: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class FirstColumn : IRouletteBetFields
    {
        public int bet { get; set; }
        public FirstColumn(int bet) { this.bet = bet; }
        public bool checkWin(RouletteField winningField)
        {
            if(winningField.number % 3 == 1)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "FirstColumn: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class SecondColumn : IRouletteBetFields
    {
        public int bet { get; set; }
        public SecondColumn(int bet) { this.bet = bet; }
        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number % 3 == 2)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "SecondColumn: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }

    public class ThirdColumn : IRouletteBetFields
    {
        public int bet { get; set; }
        public ThirdColumn(int bet) { this.bet = bet; }
        public bool checkWin(RouletteField winningField)
        {
            if (winningField.number % 3 == 0)
            {
                return true;
            }
            return false;
        }
        public int potentialWin()
        {
            return this.bet * 3;
        }

        public override string ToString()
        {
            string potentialWin = this.potentialWin().ToString();
            return "ThirdColumn: " + bet + " Potencjalna Wygrana: " + potentialWin;
        }
    }
}