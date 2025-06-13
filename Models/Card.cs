using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class Card
    {
        public string Suit { get; set; } // e.g., "Hearts", "Diamonds", "Clubs", "Spades"
        public string Rank { get; set; } // e.g., "2", "3", ..., "10", "Jack", "Queen", "King", "Ace"
        public int Value => Rank switch
        {
            "A" => 11,
            "K" or "Q" or "J" => 10,
            _ => int.Parse(Rank)
        };
        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
        
    }
}
