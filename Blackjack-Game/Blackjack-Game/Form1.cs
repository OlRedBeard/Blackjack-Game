using BlackjackClasses;

namespace Blackjack_Game
{
    public partial class Form1 : Form
    {
        ClientCommunication comm;
        public string Username = "";

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
            btnDecline.Visible = false;
            btnAccept.Visible = true;
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

                // Send username
            }
        }

        private void Comm_ReceivedMessage(string message)
        {
            throw new NotImplementedException();
        }

        private void Comm_ConnectionFailed(string servername, int port)
        {
            throw new NotImplementedException();
        }

        private void Comm_Connected(string servername, int port)
        {
            throw new NotImplementedException();
        }
    }
}