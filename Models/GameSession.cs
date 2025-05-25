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
        public int GameTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public GameSession() { }
        public GameSession(int playerId, int gameTypeId, DateTime start, DateTime end)
        {
            UserId = playerId;
            GameTypeId = gameTypeId;
            StartTime = start;
            EndTime = end;
        }
    }
}
