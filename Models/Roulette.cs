using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class Roulette
    {
        public List<RouletteField> roulleteFields = new List<RouletteField>();

        public Roulette()
        {
            create_roulette();
        }

        public void create_roulette()
        {
            roulleteFields.Add(new RouletteField(-1, "green")); // 00 -> oznaczone jako -1 (C# nie pozwala na 00 jako liczby)
            roulleteFields.Add(new RouletteField(27, "red"));
            roulleteFields.Add(new RouletteField(10, "black"));
            roulleteFields.Add(new RouletteField(25, "red"));
            roulleteFields.Add(new RouletteField(29, "black"));
            roulleteFields.Add(new RouletteField(12, "red"));
            roulleteFields.Add(new RouletteField(8, "black"));
            roulleteFields.Add(new RouletteField(19, "red"));
            roulleteFields.Add(new RouletteField(31, "black"));
            roulleteFields.Add(new RouletteField(18, "red"));
            roulleteFields.Add(new RouletteField(6, "black"));
            roulleteFields.Add(new RouletteField(21, "red"));
            roulleteFields.Add(new RouletteField(33, "black"));
            roulleteFields.Add(new RouletteField(16, "red"));
            roulleteFields.Add(new RouletteField(4, "black"));
            roulleteFields.Add(new RouletteField(23, "red"));
            roulleteFields.Add(new RouletteField(35, "black"));
            roulleteFields.Add(new RouletteField(14, "red"));
            roulleteFields.Add(new RouletteField(2, "black"));
            roulleteFields.Add(new RouletteField(0, "green"));
            roulleteFields.Add(new RouletteField(28, "black"));
            roulleteFields.Add(new RouletteField(9, "red"));
            roulleteFields.Add(new RouletteField(26, "black"));
            roulleteFields.Add(new RouletteField(30, "red"));
            roulleteFields.Add(new RouletteField(11, "black"));
            roulleteFields.Add(new RouletteField(7, "red"));
            roulleteFields.Add(new RouletteField(20, "black"));
            roulleteFields.Add(new RouletteField(32, "red"));
            roulleteFields.Add(new RouletteField(17, "black"));
            roulleteFields.Add(new RouletteField(5, "red"));
            roulleteFields.Add(new RouletteField(22, "black"));
            roulleteFields.Add(new RouletteField(34, "red"));
            roulleteFields.Add(new RouletteField(15, "black"));
            roulleteFields.Add(new RouletteField(3, "red"));
            roulleteFields.Add(new RouletteField(24, "black"));
            roulleteFields.Add(new RouletteField(36, "red"));
            roulleteFields.Add(new RouletteField(13, "black"));
            roulleteFields.Add(new RouletteField(1, "red"));
        }
    }
}
