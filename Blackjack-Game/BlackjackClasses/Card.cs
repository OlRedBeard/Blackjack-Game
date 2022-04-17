namespace BlackjackClasses
{
    public class Card
    {
        public string Suit { get; set; }
        public string Face { get; set; }
        public int Value { get; set; }

        public Card(string face, string suit, int value)
        {
            this.Face = face;
            this.Suit = suit;
            this.Value = value;
        }

        public override string ToString()
        {
            return Face.ToLower() + "_of_" + Suit.ToLower() + ".png";
        }
    }
}