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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public GameType() { }
        public GameType(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
