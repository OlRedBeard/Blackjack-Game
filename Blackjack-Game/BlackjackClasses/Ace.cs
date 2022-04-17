using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class Ace : Card
    {
        public Ace(string face, string suit, int value) : base(face, suit, value)
        {
        }

        public void SwapValue()
        {
            this.Value = 1;
        }
    }
}
