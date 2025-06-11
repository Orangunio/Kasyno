using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class RouletteField
    {
        public int number {  get; set; }
        public string color { get; set; }

        public RouletteField(int number, string color)
        {
            this.number = number; this.color = color;
        }

        public override string ToString()
        {
            if(number == -1)
            {
                return "00 ";
            }
            else
            {
                return number.ToString();
            }
        }
    }
}
