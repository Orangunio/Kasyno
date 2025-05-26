using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class GameType
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public GameType() { }
        public GameType(string name)
        {
            Name = name;
        }
    }
}
