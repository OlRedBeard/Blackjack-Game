using System.Net.Sockets;
using System.Net;
using BlackjackClasses;

namespace Blackjack_Server
{
    public partial class Server : Form
    {
        public static int port = 5000;
        TcpListener listener;
        ServerManager mngr;
        public static List<ServerManager> lstClients = new List<ServerManager>();

        public Server()
        {
            InitializeComponent();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            cmbIP.DataSource = Dns.GetHostEntry(SystemInformation.ComputerName).AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress serverName = (IPAddress)cmbIP.SelectedValue;
                listener = new TcpListener(serverName, port);
                listener.Start();

                //Create manager and wire up delegates
                mngr = new ServerManager(listener);
                mngr.NewClientConnected += Mngr_NewClientConnected;
                mngr.SetClientName += Mngr_SetClientName;
                mngr.ClientDisconnected += Mngr_ClientDisconnected;
                mngr.ReceivedMessage += Mngr_ReceivedMessage;
                mngr.ChallengeIssued += Mngr_ChallengeIssued;
                mngr.ChallengeAccepted += Mngr_ChallengeAccepted;

                lstMessages.Items.Add("** Server has Started **");
                ScrollListBox();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ServerMessage(string message)
        {
            foreach (ServerManager cli in lstClients)
            {
                cli.SendMessage(message);
            }
        }

        private void RelayMessage(ServerManager client, string message)
        {
            foreach (ServerManager cli in lstClients)
            {
                cli.SendMessage(client.name + ": " + message);
            }
        }

        private void RelayClientList()
        {
            List<string> clientList = new List<string>();

            foreach(ServerManager cli in lstClients)
            {
                clientList.Add(cli.name);
            }

            foreach (ServerManager cli in lstClients)
            {
                cli.SendClientList(clientList);
            }
        }

        private void ScrollListBox()
        {
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }

        private void Mngr_ChallengeAccepted(ServerManager client, Tuple<Challenge, int> message)
        {
            foreach (ServerManager cli in lstClients)
            {
                if (cli.name == message.Item1.Issuer)
                {
                    cli.SendMessage(message);
                }
                else if (cli.name == message.Item1.Recipient)
                {
                    cli.SendMessage(message);
                }
            }

            string msg = "** " + message.Item1.Recipient + " accepted " + message.Item1.Issuer + "'s challenge! **";
            ServerMessage(msg);
            lstMessages.Items.Add(msg);
            ScrollListBox();
        }

        private void Mngr_ChallengeIssued(ServerManager client, Challenge message)
        {
            foreach(ServerManager cli in lstClients)
            {
                if(cli.name == message.Recipient)
                {
                    cli.SendMessage(message);
                }
                else if (cli.name == message.Issuer)
                {
                    string msg = "** You challenged " + message.Recipient + " to a game! **";
                    cli.SendMessage(msg);
                }
                else
                {
                    string msg = "** " + message.Issuer + " challenged " + message.Recipient + " to a game! **";
                }
            }

            lstMessages.Items.Add("** " + message.Issuer + " challenged " + message.Recipient + " to a game! **");
            ScrollListBox();
        }

        private void Mngr_NewClientConnected(ServerManager client)
        {
            // Add the client to the list
            lstClients.Add(client);

            // Instantiate manager for client and wire up delegates
            mngr = new ServerManager(listener);
            mngr.NewClientConnected += Mngr_NewClientConnected;
            mngr.SetClientName += Mngr_SetClientName;
            mngr.ClientDisconnected += Mngr_ClientDisconnected;
            mngr.ReceivedMessage += Mngr_ReceivedMessage;
            mngr.ChallengeIssued += Mngr_ChallengeIssued;
            mngr.ChallengeAccepted += Mngr_ChallengeAccepted;
        }

        private void Mngr_ClientDisconnected(ServerManager client)
        {
            lstClients.Remove(client);
            string msg = "** " + client.name + " has disconnected. **";

            // Inform clients
            ServerMessage(msg);
            lstMessages.Items.Add(msg);
            ScrollListBox();
            RelayClientList();
        }

        private void Mngr_SetClientName(ServerManager client)
        {
            string msg = "** " + client.name + " has joined the chat! **";

            // Inform clients about the new connection
            ServerMessage(msg);
            lstMessages.Items.Add(msg);
            ScrollListBox();

            // Update the user list for all clients
            RelayClientList();
        }

        private void Mngr_ReceivedMessage(ServerManager client, string message)
        {
            RelayMessage(client, message);
            lstMessages.Items.Add(client.name + ": " + message);
            ScrollListBox();
        }
    }
}