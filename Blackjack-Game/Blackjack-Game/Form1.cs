using BlackjackClasses;

namespace Blackjack_Game
{
    public partial class Form1 : Form
    {
        ClientCommunication comm;
        public string Username = "";
        public List<string> Users = new List<string>();
        public List<Challenge> Challenges = new List<Challenge>();

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
            }
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
                ChallengeForm cf = new ChallengeForm(cmbUsers.Text);
                cf.ChallengeInfo += Cf_ChallengeInfo;
                cf.Show();
            }
        }

        private void Cf_ChallengeInfo(string opp, string ip)
        {
            Challenge ch = new Challenge(this.Username, opp, ip);
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
            lblIncChallenge.Text = "New Challenge Received!";
            cmbChallenges.DataSource = Challenges;
        }

        private void BeginGame(Challenge c)
        {
            if (c.Issuer == this.Username)
            {
                GameBoard gb = new GameBoard(c, this.Username);
                gb.Show();
            }
            else
            {
                Thread.Sleep(1000);
                GameBoard gb = new GameBoard(c, this.Username);
                gb.Show();
            }
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
    }
}