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
        public int Type;
        public int thePot = 0;
        public bool yourTurn;

        public GameBoard(int type)
        {
            InitializeComponent();

            this.Type = type;

            // Set visibility
            SetLabelValues();
            btnBegin.Visible = true;
            btnHit.Visible = false;
            btnStand.Visible = false;

            // Instantiate deck & shuffle
            theDeck = new Deck();
            theDeck.Shuffle();
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (this.Type == 0)
                PlaySP();
        }

        private void btnHit_Click(object sender, EventArgs e)
        {
            Card tmp = theDeck.DealCard();
            if (tmp is Ace && you.CardValue > 10)
            {
                Ace ace = (Ace)tmp;
                ace.SwapValue();
                tmp = ace;
            }
            you.GetCard(tmp);
            SetLabelValues();

            if (you.myCards.Count == 3)
                picYou3.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[2].ToString());
            else if (you.myCards.Count == 4)
                picYou4.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[3].ToString());
            else if (you.myCards.Count == 5)
                picYou5.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[4].ToString());
        }

        private void btnStand_Click(object sender, EventArgs e)
        {
            yourTurn = false;
        }

        public void SetLabelValues()
        {
            lblYouVal.Text = you.CardValue.ToString();
            lblOppVal.Text = ai.CardValue.ToString();
            lblYouChips.Text = you.Chips.ToString();
            lblOppChips.Text = ai.Chips.ToString();
            lblPot.Text = thePot.ToString();
        }

        public void PlaySP()
        {
            // Hide unnecesary controls
            btnBegin.Visible = false;

            // Deal starting cards
            for (int i = 0; i < 4; i++)
            {
                Card tmp = theDeck.DealCard();
                if (i % 2 == 0)
                    you.GetCard(tmp);
                else
                    ai.GetCard(tmp);

                SetLabelValues();
            }

            // Update picture boxes
            picYou1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[0].ToString());
            picOpp1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + ai.myCards[0].ToString());
            picYou2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[1].ToString());
            picOpp2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + ai.myCards[1].ToString());

            // Player Turn
            yourTurn = true;
            PlayerTurn();

            // Opponent turn
            
        }

        public void PlayerTurn()
        {
            // Player turn -- NOT WORKING
            btnHit.Visible = true;
            btnStand.Visible = true;

            while (yourTurn)
            {
                if (you.CardValue > 21)
                {
                    EndGame(0);
                    break;
                }                    
                else if (you.myCards.Count == 5)
                {
                    EndGame(1);
                    break;
                }
            }

            btnHit.Visible = false;
            btnStand.Visible = false;
        }

        public void EndGame(int mod)
        {
            switch (mod)
            {
                case 0:
                    lblPot.Text = "You Lose!";
                    ai.TakePot(thePot);
                    thePot = 0;
                    break;
            }
        }
    }
}
