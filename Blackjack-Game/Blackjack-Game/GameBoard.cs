using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlackjackClasses;

namespace Blackjack_Game
{
    public partial class GameBoard : Form
    {
        public Deck theDeck;
        public Player you = new Player();
        public PlayerAI ai = new PlayerAI();

        public GameBoard(int type)
        {
            InitializeComponent();

            // Set labels
            

            // Instantiate deck & shuffle
            theDeck = new Deck();
            theDeck.Shuffle();

            // Type 0 = single player, type 1 = host, type 2 = join
            if (type == 0)
                PlaySP();
        }

        public void PlaySP()
        {
            // Deal starting cards
            for (int i = 0; i < 4; i++)
            {
                Card tmp = theDeck.DealCard();
                if (i % 2 == 0)
                    you.GetCard(tmp);
                else
                    ai.GetCard(tmp);

                lblYouVal.Text = you.CardValue.ToString();
                lblOppVal.Text = ai.CardValue.ToString();
            }

            // Update picture boxes
            //picYou1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[0].ToString());
            //picOpp1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + ai.myCards[0].ToString());
            //picYou2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[1].ToString());
            //picOpp2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + ai.myCards[1].ToString());

            // Player turn
            int hs = 0;
            while (hs < 1)
            {

            }
        }
    }
}
