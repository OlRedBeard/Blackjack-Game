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

        private void ScrollListBox()
        {
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }

        private void Mngr_NewClientConnected(ServerManager client)
        {
            // Add the client to the list
            lstClients.Add(client);

            // Instantiate manager for client and wire up delegates
        }

        private void Mngr_ClientDisconnected(ServerManager client)
        {
            throw new NotImplementedException();
        }

        private void Mngr_SetClientName(ServerManager client)
        {
            string msg = "** " + client.name + " has joined the chat! **";

            // Inform clients about the enw connection
            ServerMessage(msg);
            lstMessages.Items.Add(msg);
            ScrollListBox();
        }
    }
}