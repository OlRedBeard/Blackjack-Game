using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlackjackClasses
{
    public class ClientCommunication
    {
        public string serverName;
        public int port = 5000;

        private TcpClient client;
        private NetworkStream nStream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private BackgroundWorker worker = new BackgroundWorker();

        public event ConnectedEventHandler Connected;
        public delegate void ConnectedEventHandler(string servername, int port);

        public event ConnectionFailedEventHandler ConnectionFailed;
        public delegate void ConnectionFailedEventHandler(string servername, int port);

        public event ReceivedMessageEventHandler ReceivedMessage;
        public delegate void ReceivedMessageEventHandler(string message);

        public event UserListUpdateEventHandler UserListUpdate;
        public delegate void UserListUpdateEventHandler(List<string> message);

        public event ChallengeReceivedEventHandler ChallengeReceived;
        public delegate void ChallengeReceivedEventHandler(Challenge message);

        public event HostGameEventHandler StartGame;
        public delegate void HostGameEventHandler(Tuple<Challenge, int> message);

        public ClientCommunication(string serv)
        {
            this.serverName = serv;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        public void SendMessage(string msg)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, msg);
        }

        public void SendChallenge(Challenge challengeInfo)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, challengeInfo);
        }

        public void RespondToChallenge(Challenge c, int response)
        {
            Tuple<Challenge, int> challengeTuple = new Tuple<Challenge, int>(c, response);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer.BaseStream, challengeTuple);
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
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
                    while (true)
                    {

                        IFormatter formatter = new BinaryFormatter();
                        object o = (object)formatter.Deserialize(nStream);

                        // Check object types
                        if (o is string)
                        {
                            ReceivedMessage((string)o);
                        }
                        else if (o is List<string>)
                        {
                            UserListUpdate((List<string>)o);
                        }
                        else if (o is Challenge)
                        {
                            ChallengeReceived((Challenge)o);
                        }
                        else if (o is Tuple<Challenge, int>)
                        {
                            StartGame((Tuple<Challenge, int>)o);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch
            {
                ConnectionFailed(this.serverName, this.port);
            }
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
