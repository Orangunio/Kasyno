using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class Deck
    {
        public List<Card> Cards { get; set; }
        public Deck()
        {
            Cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    Cards.Add(new Card(suit, rank));
                }
            }
            Shuffle();
        }
        public void Shuffle()
        {
            Random rand = new Random();
            Cards = Cards.OrderBy(c => rand.Next()).ToList();
        }
        public Card DrawCard()
        {
            if (Cards.Count == 0) return null;
            Card drawnCard = Cards[0];
            Cards.RemoveAt(0);
            return drawnCard;
        }
    }
}
