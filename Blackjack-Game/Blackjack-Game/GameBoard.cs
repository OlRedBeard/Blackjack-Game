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
        public Player opponent;
        public int Type;
        public int thePot = 0;
        public bool yourTurn;
        public bool oppturn;
        public bool roundOver;
        BackgroundWorker yourWorker = new BackgroundWorker();
        BackgroundWorker oppWorker = new BackgroundWorker();

        public GameBoard(int type)
        {
            InitializeComponent();

            this.Type = type;

            // Instantiate AI player if needed
            if (Type == 0)
            {
                opponent = new PlayerAI();
                btnBegin.Visible = true;

                // Set visibility
                SetLabelValues();
                btnHit.Visible = false;
                btnStand.Visible = false;

                // Instantiate deck & shuffle
                theDeck = new Deck();
            }                      
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "BEGIN")
            {
                if (this.Type == 0)
                    PlaySP();
            }
            else if (btnBegin.Text == "Next Round")
            {
                ClearResults();
                lblTitle.Text = "POT";
                lblTitle.Visible = true;

                if (this.Type == 0)
                    PlaySP();
            }
        }

        private void ClearResults()
        {
            picYou1.Image = null;
            picYou2.Image = null;
            picYou3.Image = null;
            picYou4.Image = null;
            picYou5.Image = null;
            picOpp1.Image = null;
            picOpp2.Image = null;
            picOpp3.Image = null;
            picOpp4.Image = null;
            picOpp5.Image = null;

            you.myCards.Clear();
            you.CardValue = 0;
            opponent.myCards.Clear();
            opponent.CardValue = 0;
        }

        private void btnHit_Click(object sender, EventArgs e)
        {
            Card tmp = theDeck.DealCard();
            if (tmp is Ace && you.CardValue >= 11)
            {
                Ace ace = (Ace)tmp;
                ace.SwapValue();
                tmp = ace;
            }
            else if (tmp.Value + you.CardValue > 21)
            {
                foreach (Card tmp2 in you.myCards)
                {
                    if (tmp2 is Ace)
                    {
                        Ace ace2 = (Ace)tmp2;
                        if (ace2.Value == 11)
                        {
                            ace2.SwapValue();
                            you.CardValue -= 10;
                        }
                    }
                }
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
            lblOppVal.Text = opponent.CardValue.ToString();
            lblYouChips.Text = you.Chips.ToString();
            lblOppChips.Text = opponent.Chips.ToString();
        }

        public void PlaySP()
        {
            roundOver = false;
            theDeck.Shuffle();

            // Hide unnecesary controls
            btnBegin.Visible = false;

            // Deal starting cards
            for (int i = 0; i < 4; i++)
            {
                Card tmp = theDeck.DealCard();
                if (i % 2 == 0)
                    you.GetCard(tmp);
                else
                    opponent.GetCard(tmp);

                SetLabelValues();
                lblPot.Text = thePot.ToString();
            }

            // Update picture boxes
            picYou1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[0].ToString());
            picOpp1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[0].ToString());
            picYou2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[1].ToString());
            picOpp2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[1].ToString());

            // Player Turn
            yourTurn = true;
            yourWorker.WorkerSupportsCancellation = true;
            yourWorker.DoWork += YourWorker_DoWork;
            yourWorker.RunWorkerCompleted += YourWorker_RunWorkerCompleted;
            yourWorker.RunWorkerAsync();
            PlayerTurn();            
        }

        public void PlayerTurn()
        {
            thePot += you.Bet(10);
            thePot += opponent.Bet(10);
            SetLabelValues();
            lblPot.Text = thePot.ToString();
            btnHit.Visible = true;
            btnHit.Enabled = true;
            btnStand.Visible = true;
        }

        public void OpponentTurn()
        {
            oppturn = true;
            lblPot.Text = thePot.ToString();

            if (Type == 0)
            {                
                oppWorker.WorkerSupportsCancellation = true;
                oppWorker.DoWork += OppWorker_DoWork;
                oppWorker.RunWorkerCompleted += OppWorker_RunWorkerCompleted;
                if (!oppWorker.IsBusy)
                    oppWorker.RunWorkerAsync();
            }
        }

        public void OpponentHit()
        {
            if (Type == 0)
            {
                Card tmp = theDeck.DealCard();

                if (tmp is Ace && opponent.CardValue >= 11)
                {
                    Ace ace = (Ace)tmp;
                    ace.SwapValue();
                    tmp = ace;
                }
                else if (tmp.Value + opponent.CardValue > 21)
                {
                    foreach (Card tmp2 in opponent.myCards)
                    {
                        if (tmp2 is Ace)
                        {
                            Ace ace2 = (Ace)tmp2;
                            if (ace2.Value == 11)
                            {
                                ace2.SwapValue();
                                opponent.CardValue -= 10;
                            }
                        }
                    }
                }

                opponent.GetCard(tmp);
                SetLabelValues();

                if (opponent.myCards.Count == 3)
                    picOpp3.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[2].ToString());
                else if (opponent.myCards.Count == 4)
                    picOpp4.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[3].ToString());
                else if (opponent.myCards.Count == 5)
                    picOpp5.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[4].ToString());

                OpponentCheck();
            }
            else
            {

            }
        }

        public void OpponentCheck()
        {
            if (opponent.CardValue > 21)
            {
                EndGame(1);
            }
            else if (opponent.myCards.Count == 5)
            {
                EndGame(3);
            }
        }

        private void OppWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            SetLabelValues();
        }

        private void OppWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (oppturn)
            {
                if (opponent is PlayerAI)
                {
                    PlayerAI playerAI = (PlayerAI)opponent;
                    int x = playerAI.MakeMove(you.CardValue);

                    if (x == 1)
                        this.Invoke(new Action(() => EndGame(3)));
                    else
                    {
                        // Invoke method to add a card to the opponent's list and check for win/loss
                        this.Invoke(new Action(() => OpponentHit()));
                    }
                }
                else
                {

                }
            }
        }

        public void EndGame(int mod)
        {
            yourTurn = false;
            oppturn = false;
            roundOver = true;
            lblTitle.Visible = false;

            switch (mod)
            {
                case 0:
                    opponent.TakePot(thePot);
                    thePot = 0;
                    SetLabelValues();
                    lblPot.Text = "You Lose!";
                    break;
                case 1:
                    you.TakePot(thePot);
                    thePot = 0;
                    SetLabelValues();
                    lblPot.Text = "You Win!";
                    break;
                case 3:
                    if (you.CardValue > opponent.CardValue)
                    {
                        you.TakePot(thePot);
                        thePot = 0;
                        SetLabelValues();
                        lblPot.Text = "You Win!";
                    }
                        
                    else if (you.CardValue < opponent.CardValue)
                    {
                        opponent.TakePot(thePot);
                        thePot = 0;
                        SetLabelValues();
                        lblPot.Text = "You Lose!";
                    }                        
                    else
                    {                        
                        you.TakePot(thePot / 2);
                        opponent.TakePot(thePot / 2);
                        thePot = 0;
                        lblPot.Text = "Push!";
                    }
                    break;
            }

            if (you.Chips > 0 && opponent.Chips > 0)
            {
                btnBegin.Text = "Next Round";
                btnBegin.Visible = true;
            }
            else if (you.Chips == 0)
            {
                lblTitle.Text = "Game Over";
                lblTitle.Visible = true;
            }
        }

        private void YourWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            btnHit.Visible = false;
            btnStand.Visible = false;
            if (!roundOver)
                OpponentTurn();
        }

        private void YourWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while (yourTurn)
            {
                if (you.CardValue > 21)
                {
                    this.Invoke(new Action(() => EndGame(0)));                    
                    break;
                }
                else if (you.myCards.Count == 5)
                {
                    this.Invoke(new Action(() => btnHit.Enabled = false));
                    break;
                }
            }
        }
    }
}
