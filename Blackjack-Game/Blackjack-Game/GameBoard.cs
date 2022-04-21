using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlackjackClasses;

namespace Blackjack_Game
{
    public partial class GameBoard : Form
    {
        public static int port = 6000;
        TcpListener listener;
        GameServer gs;
        GameComm gc;
        public static List<GameServer> clients = new List<GameServer> ();

        public Deck theDeck;
        public Player you = new Player();
        public Player opponent;
        public Challenge theChallenge;
        public int Type;
        public int thePot = 0;
        public bool yourTurn;
        public bool oppturn;
        public bool roundOver;
        BackgroundWorker yourWorker = new BackgroundWorker();
        BackgroundWorker oppWorker = new BackgroundWorker();

        public event GameOverEventHandler GameOver;
        public delegate void GameOverEventHandler(Challenge ch);

        public GameBoard(int type)
        {
            InitializeComponent();

            this.Type = type;

            opponent = new PlayerAI();
            btnBegin.Visible = true;

            // Set visibility
            SetLabelValues();
            btnHit.Visible = false;
            btnStand.Visible = false;

            // Instantiate deck
            theDeck = new Deck();
        }

        public GameBoard(Challenge c, string user)
        {
            InitializeComponent();
            this.theChallenge = c;
            you.SetName(user);

            if (theChallenge.Issuer == you.Name)
            {
                this.Type = 1;
                this.Text = "Blackjack - Host";
                opponent = new Player();
                opponent.SetName(theChallenge.Recipient);
                SetupServer(theChallenge.IssuerIP);
            }
            else
            {
                this.Type = 2;
                this.Text = "Blackjack - Guest";
                opponent = new Player();
                opponent.SetName(theChallenge.Issuer);
            }

            // Set visibility
            SetLabelValues();
            btnBegin.Visible = false;
            btnHit.Visible = false;
            btnStand.Visible = false;
        }

        private void SetupComms(string ip)
        {
            gc = new GameComm(ip);
            gc.Connected += Gc_Connected;
            gc.PlayerCard += Gc_PlayerCard;
            gc.OpponentCard += Gc_OpponentCard;
            gc.UpdateOpponent += Gc_UpdateOpponent;
            gc.YourTurn += Gc_YourTurn;
            gc.HostLeft += Gc_HostLeft;
        }

        private void Gc_HostLeft()
        {
            MessageBox.Show("Host left, Game Ending");
            this.Invoke(new Action(() => this.Close()));
        }

        private void YourTurn(bool yourTurn)
        {
            this.yourTurn = yourTurn;
            yourWorker.WorkerSupportsCancellation = true;
            yourWorker.DoWork += YourWorker_DoWork;
            yourWorker.RunWorkerCompleted += YourWorker_RunWorkerCompleted;
            yourWorker.RunWorkerAsync();
            PlayerTurn();
        }

        private void Gc_YourTurn(bool yourTurn)
        {
            this.Invoke(new Action(() => YourTurn(yourTurn)));
        }

        private void Gc_UpdateOpponent(Player opponent)
        {
            this.Invoke(new Action(() => UpdateOpponent(opponent)));
        }

        private void Gc_OpponentCard(Card card)
        {
            this.Invoke(new Action(() => UpdateOppCards(card)));
        }

        private void Gc_PlayerCard(Card card)
        {
            this.Invoke(new Action(() => UpdateMyCards(card)));
        }

        private void Gc_Connected(string servername, int port)
        {
            if (IsHandleCreated)
                this.Invoke(new Action(() => PlayGuest()));
        }

        private void UpdateOpponent(Player opp)
        {
            this.opponent = opp;
        }

        private void UpdateOppCards(Card card)
        {
            if (opponent.myCards.Count == 0)
            {
                ClearPictures();
                lblPot.Visible = false;
            }

            if (card is Ace && opponent.CardValue >= 11)
            {
                Ace ace = (Ace)card;
                ace.SwapValue();
                card = ace;
            }
            else if (card.Value + opponent.CardValue > 21)
            {
                foreach (Card tmp2 in opponent.myCards)
                {
                    if (tmp2 is Ace && card.Value + opponent.CardValue > 21)
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

            opponent.GetCard(card);
            SetLabelValues();

            if (opponent.myCards.Count == 1)
                picOpp1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[0].ToString());
            else if (opponent.myCards.Count == 2)
                picOpp2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[1].ToString());
            else if (opponent.myCards.Count == 3)
                picOpp3.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[2].ToString());
            else if (opponent.myCards.Count == 4)
                picOpp4.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[3].ToString());
            else if (opponent.myCards.Count == 5)
                picOpp5.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + opponent.myCards[4].ToString());

            OpponentCheck();
        }
        
        private void UpdateMyCards(Card card)
        {
            if (card is Ace && you.CardValue >= 11)
            {
                Ace ace = (Ace)card;
                ace.SwapValue();
                card = ace;
            }
            else if (card.Value + you.CardValue > 21)
            {
                foreach (Card tmp2 in you.myCards)
                {
                    if (tmp2 is Ace && card.Value + you.CardValue > 21)
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

            you.GetCard(card);
            SetLabelValues();

            if (you.myCards.Count == 1)
                picYou1.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[0].ToString());
            else if (you.myCards.Count == 2)
                picYou2.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[1].ToString());
            else if (you.myCards.Count == 3)
                picYou3.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[2].ToString());
            else if (you.myCards.Count == 4)
                picYou4.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[3].ToString());
            else if (you.myCards.Count == 5)
                picYou5.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[4].ToString());

            if (you.CardValue > 21)
            {
                gc.EndGame(you);
                EndGame(0);
            }                
            else if (you.myCards.Count == 5)
                btnHit.Enabled = false;
        }

        private void PlayGuest()
        {
            lblOppName.Text = theChallenge.Issuer;
            SetLabelValues();
            lblPot.Text = thePot.ToString();
        }

        private void SetupServer(string ip)
        {
            try
            {
                IPAddress serverName = (IPAddress)IPAddress.Parse(ip);
                listener = new TcpListener(serverName, port);
                listener.Start();

                // Instantiate server and wire up delegates
                gs = new GameServer(listener);
                gs.OpponenentConnected += Gs_OpponenentConnected;
                gs.OpponentUpdate += Gs_OpponentUpdate;
                gs.RequestCard += Gs_RequestCard;
                gs.ClientLeft += Gs_ClientLeft;

                // Setup labels
                lblTitle.Visible = false;
                lblPot.Text = "Waiting for Opponent";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Gs_RequestCard()
        {
            Card tmp = theDeck.DealCard();
            UpdateOppCards(tmp);
            SendCardInfo(new Tuple<Card,int>(tmp, 1));
        }

        private void Gs_OpponentUpdate(Player opponent)
        {
            this.opponent = opponent;
            if (opponent.CardValue > 21)
            {
                EndGame(1);
            }
            else
            {
                EndGame(3);
            }                
        }

        private void Gs_OpponenentConnected(GameServer client)
        {
            clients.Add(client);
            lblOppName.Text = opponent.Name;
            lblTitle.Visible = true;
            lblPot.Text = thePot.ToString();
            btnBegin.Visible = true;

            gs = new GameServer(listener);
            gs.OpponenentConnected += Gs_OpponenentConnected;
            gs.OpponentUpdate += Gs_OpponentUpdate;
            gs.RequestCard += Gs_RequestCard;
            gs.ClientLeft += Gs_ClientLeft;
        }

        private void Gs_ClientLeft()
        {
            MessageBox.Show("Client Left, Game Ending.");
            this.Close();
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "BEGIN")
            {
                if (this.Type == 0)
                    PlaySP();
                else if (this.Type == 1)
                    PlayHost();
            }
            else if (btnBegin.Text == "Next Round")
            {
                ClearResults();
                lblTitle.Text = "POT";
                lblTitle.Visible = true;

                if (this.Type == 0)
                    PlaySP();
                else if (this.Type == 1)
                    PlayHost();
            }
        }

        private void SendGameEnd()
        {
            foreach (GameServer gserv in clients)
            {
                gserv.SendEnd();
            }
        }

        private void SendCardInfo(Tuple<Card, int> msg)
        {
            foreach (GameServer gserv in clients)
            {
                gserv.SendCardInfo(msg);
            }
        }

        private void SendPlayerInfo(Player p)
        {
            foreach (GameServer gserv in clients)
            {
                gserv.SendPlayerInfo(p);
            }
        }

        private void PlayHost()
        {
            theDeck = new Deck();
            theDeck.Shuffle();

            btnBegin.Visible = false;

            // Deal starting cards
            for (int i = 0; i < 4; i++)
            {
                Card tmp = theDeck.DealCard();
                if (i % 2 == 0)
                {
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
                    SendCardInfo(new Tuple<Card, int>(tmp, 0));
                }
                else
                {
                    opponent.GetCard(tmp);
                    SendCardInfo(new Tuple<Card, int>(tmp, 1));
                }

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

        private void ClearPictures()
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
        }

        private void ClearValues()
        {
            you.myCards.Clear();
            you.CardValue = 0;
            opponent.myCards.Clear();
            opponent.CardValue = 0;
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
            if (Type < 2)
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

                if (Type == 1)
                {
                    SendCardInfo(new Tuple<Card, int>(tmp, 0));
                    SendPlayerInfo(you);
                }

                if (you.myCards.Count == 3)
                    picYou3.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[2].ToString());
                else if (you.myCards.Count == 4)
                    picYou4.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[3].ToString());
                else if (you.myCards.Count == 5)
                    picYou5.Image = Image.FromFile(Environment.CurrentDirectory + "/images/" + you.myCards[4].ToString());
            }
            else
            {
                gc.RequestCard(0);
            }            
        }

        private void EndTurn()
        {
            foreach (GameServer gserv in clients)
            {
                gserv.EndTurn();
            }
        }

        private void btnStand_Click(object sender, EventArgs e)
        {
            if (Type == 0)
                yourTurn = false;
            else if (Type == 1)
            {
                yourTurn = false;
                EndTurn();
            }
            else
            {
                gc.EndGame(you);
                EndGame(3);
            }
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
            lblPot.Visible = true;
            lblPot.Text = thePot.ToString();
            lblTitle.Text = "THE POT";
            lblTitle.Visible = true;
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
                lblTitle.Text = "Waiting for Host...";
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

            if (Type < 2)
            {
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
                else if (opponent.Chips == 0)
                {
                    lblTitle.Text = "Game Over";
                    lblTitle.Visible = true;
                }
            }
            else
            {
                if (you.Chips > 0 && opponent.Chips > 0)
                {
                    lblTitle.Text = "Waiting";
                    lblTitle.Visible = true;
                }
                else if (you.Chips == 0)
                {
                    lblTitle.Text = "Game Over";
                    lblTitle.Visible = true;
                }
                else if (opponent.Chips == 0)
                {
                    lblTitle.Text = "Game Over";
                    lblTitle.Visible = true;
                }

                ClearValues();
            }
            
        }

        private void YourWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            btnHit.Visible = false;
            btnStand.Visible = false;
            if (Type == 0)
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

        private void GameBoard_Load(object sender, EventArgs e)
        {
            if (Type == 2)
                SetupComms(theChallenge.IssuerIP);
        }

        private void GameBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Type == 0)
            {
                this.Close();
            }
            else if (Type == 1)
            {
                GameOver(this.theChallenge);
                SendGameEnd();
                listener.Stop();
            }
            else
            {
                gc.LeftGame("a");
                gc.done = true;
                //GameOver(this.theChallenge);
            }
        }
    }
}
