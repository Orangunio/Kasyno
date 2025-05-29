using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class GameSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int UserId { get; set; }
        [Indexed]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public GameSession() { }
        public GameSession(int playerId, DateTime start, DateTime end)
        {
            UserId = playerId;
            StartTime = start;
            EndTime = end;
        }
    }
}
