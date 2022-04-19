using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class Player
    {
        public int Chips { get; set; }
        public string Name { get; set; }

        public int CardValue = 0;
        public List<Card> myCards;

        public Player()
        {
            this.Chips = 100;
            this.CardValue = 0;
            this.myCards = new List<Card>();
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public int Bet(int bet)
        {
            if (bet > this.Chips)
                return this.Chips;
            else
            {
                this.Chips -= bet;
                return bet;
            }            
        }

        public void GetCard(Card c)
        {
            this.myCards.Add(c);
            this.CardValue += c.Value;
        }

        public void TakePot(int pot)
        {
            this.Chips += pot;
        }

        public void Reset()
        {
            this.CardValue = 0;
        }
    }
}
