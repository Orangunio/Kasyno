using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class Result
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Outcome { get; set; }
        public int Amount { get; set; } 
        public Result() { }
        public Result(string outcome, int amount)
        {
            Outcome = outcome;
            Amount = amount;
        }
    }
}
