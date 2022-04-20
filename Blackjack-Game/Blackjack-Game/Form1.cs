using BlackjackClasses;

namespace Blackjack_Game
{
    public partial class Form1 : Form
    {
        ClientCommunication comm;
        public string Username = "";
        public List<string> Users = new List<string>();
        public List<Challenge> Challenges = new List<Challenge>();
        public List<Challenge> SentChallenges = new List<Challenge>();

        public Form1()
        {
            InitializeComponent();
            pnlChat.Visible = false;
        }

        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            GameBoard gb = new GameBoard(0);
            this.Hide();
            gb.ShowDialog();
            this.Show();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            pnlChat.Visible = true;
            lblIncChallenge.Visible = false;
            btnCancelCh.Visible = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
                MessageBox.Show("Please provide username.");
            else if (txtUsername.Text.Count() > 8)
                MessageBox.Show("Username must be maximum 8 characters");
            else if (txtServIP.Text == "")
                MessageBox.Show("Enter IP Address of the server");
            else
            {
                // Send username to form field
                this.Username = txtUsername.Text;

                // Attempt connection and wire up delegates
                comm = new ClientCommunication(txtServIP.Text);
                comm.Connected += Comm_Connected;
                comm.ConnectionFailed += Comm_ConnectionFailed;
                comm.ReceivedMessage += Comm_ReceivedMessage;
                comm.UserListUpdate += Comm_UserListUpdate;
                comm.ChallengeReceived += Comm_ChallengeReceived;
                comm.StartGame += Comm_StartGame;
                comm.EndChallenge += Comm_EndChallenge;
            }
        }

        private void EndChallenge(Challenge c)
        {
            comm.RespondToChallenge(c, 0);
        }

        private void DeleteChallenge(Challenge c)
        {
            if (c.Issuer == Username)
            {
                SentChallenges.Clear();
                btnCancelCh.Visible = false;
            }
                
            else if (c.Recipient == Username)
            {
                Challenge x = null;

                foreach (Challenge ch in Challenges)
                {
                    if (ch.Issuer == c.Issuer)
                        x = c;
                }

                if (x != null)
                {
                    //List<Challenge> tmp = Challenges;
                    Challenge z = Challenges.Where(y => y.Issuer == c.Issuer).FirstOrDefault();
                    Challenges.Remove(z);
                    //Challenges = tmp;
                }
                    

                cmbChallenges.DataSource = null;
                cmbChallenges.DataSource = Challenges;

                if (Challenges.Count == 0)
                    lblIncChallenge.Text = "";
            }
        }

        private void Comm_EndChallenge(Tuple<Challenge, int> message)
        {
            this.Invoke(new Action(() => DeleteChallenge(message.Item1)));
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                comm.SendMessage(txtMessage.Text);
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (cmbUsers.SelectedItem != null)
            {
                bool exists = false;
                if (SentChallenges.Count < 1)
                {
                    ChallengeForm cf = new ChallengeForm(cmbUsers.Text);
                    cf.ChallengeInfo += Cf_ChallengeInfo;
                    cf.Show();
                }
                else
                    MessageBox.Show("Only one challenge at a time");
            }
        }

        private void Cf_ChallengeInfo(string opp, string ip)
        {
            Challenge ch = new Challenge(this.Username, opp, ip);
            this.Invoke(new Action(() => SentChallenges.Add(ch)));
            this.Invoke(new Action(() => btnCancelCh.Visible = true));
            this.Invoke(new Action(() => comm.SendChallenge(ch)));
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (cmbChallenges.SelectedItem != null)
            {
                Challenge ch = (Challenge)cmbChallenges.SelectedItem;
                comm.RespondToChallenge(ch, 1);
            }
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (cmbChallenges.SelectedItem != null)
            {
                Challenge ch = (Challenge)cmbChallenges.SelectedItem;
                comm.RespondToChallenge(ch, 0);
                //Challenges.Remove(ch);
            }
        }

        private void PopulateCmb(List<string> userList)
        {
            cmbUsers.DataSource = null;
            Users.Clear();
            
            foreach (string user in userList)
            {
                if (user != this.Username)
                    Users.Add(user);
            }

            cmbUsers.DataSource = Users;
            if (cmbUsers.Items.Count > 0)
                cmbUsers.SelectedIndex = 0;
        }

        private void ScrollMessageList()
        {
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }

        private void HideConnectionLabels(string message)
        {
            lblServIP.Visible = false;
            txtServIP.Visible = false;
            btnStart.Visible = false;
            lstMessages.Items.Add(message);
            ScrollMessageList();
        }

        private void ChallengeReceived(Challenge c)
        {
            Challenges.Add(c);
            lblIncChallenge.Visible = true;
            if (Challenges.Count == 1)
                lblIncChallenge.Text = $"{Challenges.Count} New Challenge!";
            else
                lblIncChallenge.Text = $"{Challenges.Count} New Challenges!";
            cmbChallenges.DataSource = null;
            cmbChallenges.DataSource = Challenges;
        }

        private void BeginGame(Challenge c)
        {
            if (c.Issuer == this.Username)
            {
                GameBoard gb = new GameBoard(c, this.Username);
                gb.GameOver += Gb_GameOver;
                gb.Show();
            }
            else
            {
                Thread.Sleep(1000);
                GameBoard gb = new GameBoard(c, this.Username);
                //gb.GameOver += Gb_GameOver;
                gb.Show();
            }
        }

        private void Gb_GameOver(Challenge ch)
        {
            this.Invoke(new Action(() => EndChallenge(ch)));
        }

        private void Comm_StartGame(Tuple<Challenge, int> message)
        {
            this.Invoke(new Action(() => BeginGame(message.Item1)));
        }

        private void Comm_ChallengeReceived(Challenge message)
        {
            this.Invoke((Action)(() => ChallengeReceived(message)));
        }

        private void Comm_ReceivedMessage(string message)
        {
            this.Invoke(new Action(() => lstMessages.Items.Add(message)));
        }

        private void Comm_ConnectionFailed(string servername, int port)
        {
            string msg = "** Connection lost to: " + servername + " on port " + port + " **";
            this.Invoke(new Action(() =>
                lstMessages.Items.Add(msg)
            ));
        }

        private void Comm_Connected(string servername, int port)
        {
            string msg = "** Connected to: " + servername + " on port " + port + " **";
            string setName = "!name|" + txtUsername.Text;

            this.Invoke(new Action(() =>
                HideConnectionLabels(msg)
            ));

            comm.SendMessage(setName);
        }

        private void Comm_UserListUpdate(List<string> message)
        {
            this.Invoke(new Action(() => 
                PopulateCmb(message)
            ));
        }

        private void btnCancelCh_Click(object sender, EventArgs e)
        {
            if (SentChallenges.Count > 0)
            {
                EndChallenge(SentChallenges[0]);
                btnCancelCh.Visible = false;
            }
        }
    }
}