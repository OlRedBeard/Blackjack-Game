using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class GameComm
    {
        public string serverName;
        public int port = 6000;

        private TcpClient client;
        private NetworkStream nStream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private BackgroundWorker bgw = new BackgroundWorker();

        public event ConnectedEventHandler Connected;
        public delegate void ConnectedEventHandler(string servername, int port);

        public event OpponentCardEventHandler OpponentCard;
        public delegate void OpponentCardEventHandler(Card card);

        public event PlayerCardEventHandler PlayerCard;
        public delegate void PlayerCardEventHandler(Card card);

        public event UpdateOpponentEventHandler UpdateOpponent;
        public delegate void UpdateOpponentEventHandler(Player opponent);

        public event OpponentDoneEventHandler YourTurn;
        public delegate void OpponentDoneEventHandler(bool yourTurn);

        public bool done = false;

        public GameComm(string servername)
        {
            this.serverName = servername;
            bgw.WorkerReportsProgress = true;
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerAsync();
        }

        public void LeftGame(string y)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, y);
        }

        public void RequestCard(int x)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, x);
        }

        public void EndGame(Player you)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, you);
        }

        private void Bgw_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(this.serverName, this.port);
                nStream = client.GetStream();
                reader = new BinaryReader(nStream);
                writer = new BinaryWriter(nStream);

                Connected(this.serverName, this.port);

                try
                {
                    while (!done)
                    {
                        IFormatter formatter = new BinaryFormatter();
                        object o = (object)formatter.Deserialize(nStream);

                        if (o is Tuple<Card, int>)
                        {
                            Tuple<Card, int> tuple = (Tuple<Card, int>)o;
                            if (tuple.Item2 == 0)
                            {
                                OpponentCard(tuple.Item1);
                            }
                            else
                            {
                                PlayerCard(tuple.Item1);
                            }
                        }
                        else if (o is Player)
                        {

                        }
                        else if (o is int)
                        {
                            int tmp = (int)o;

                            if (tmp == 1)
                            {
                                YourTurn(true);
                            }
                        }
                    }
                    done = true;
                }
                catch
                {
                    done = true;
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
