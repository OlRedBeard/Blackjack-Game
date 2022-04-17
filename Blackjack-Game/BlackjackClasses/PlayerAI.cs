using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class PlayerAI : Player
    {
        public int MakeMove(int oppVal)
        {
            if (this.CardValue < 17)
                return 0;
            else if (this.CardValue < oppVal)
                return 0;
            else
                return 1;
        }
    }
}
