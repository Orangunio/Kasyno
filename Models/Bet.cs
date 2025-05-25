using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class Bet
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int GameSessionId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; } // Np. róletka to było by wpisane że na czerwony czy coś takiego
        [Indexed]
        public int ResultId { get; set; }
        public Bet() { }
        public Bet(int gameSessionId, int amount, int resultId, string description)
        {
            GameSessionId = gameSessionId;
            Amount = amount;
            ResultId = resultId;
            Description = description;
        }
    }
}
