using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class Deck
    {
        public static Random rnd = new Random();
        public int counter;
        public string[] Suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
        public string[] Faces = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
            counter = 0;

            // Build deck
            foreach (string s in Suits)
            {
                foreach (string face in Faces)
                {
                    int val = 0;

                    if (face == "Ace")
                    {
                        Ace tmp = new Ace(face, s, 11);
                        Cards.Add(tmp);
                    }
                    else if (face == "Jack" || face == "Queen" || face == "King")
                    {
                        val = 10;
                        Card c = new Card(face, s, val);
                        Cards.Add(c);
                    }
                    else
                    {
                        val = int.Parse(face);
                        Card tmp = new Card(face, s, val);
                        Cards.Add(tmp);
                    }
                }
            }
        }

        public void Shuffle()
        {
            for (int i = 0; i < 1000; i++)
            {
                int a = rnd.Next(0, Cards.Count);
                int b = rnd.Next(0, Cards.Count);

                Card tmp = Cards[a];
                Cards[a] = Cards[b];
                Cards[a] = tmp;
            }

            counter = 0;
        }

        public Card DealCard()
        {
            Card tmp = Cards[counter];
            counter++;
            return tmp;
        }
    }
}
