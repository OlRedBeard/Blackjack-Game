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
    public partial class ChallengeForm : Form
    {
        public string Opponent = "";

        public event ChallengeEventHandler ChallengeInfo;
        public delegate void ChallengeEventHandler(string opp, string ip);

        public ChallengeForm(string opp)
        {
            InitializeComponent();
            this.Opponent = opp;
            lblContext.Text = "Send Challenge to " + Opponent;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ChallengeForm_Load(object sender, EventArgs e)
        {
            cmbIP.DataSource = Dns.GetHostEntry(SystemInformation.ComputerName).AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChallenge_Click(object sender, EventArgs e)
        {
            if (cmbIP.SelectedItem != null)
            {
                ChallengeInfo(this.Opponent, cmbIP.Text);
                this.Close();
            }
        }
    }
}
