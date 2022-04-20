using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

namespace BlackjackClasses
{
    public class ServerManager
    {
        public string name = "";
        private string latest = "";

        public static TcpListener listener;

        private BackgroundWorker serverWorker = new BackgroundWorker();
        private Socket connection;
        private NetworkStream socketStream;
        private BinaryReader reader;
        private BinaryWriter writer;
        bool done = false;

        public event NewClientConnectedEventHandler NewClientConnected;
        public delegate void NewClientConnectedEventHandler(ServerManager client);

        public event SetClientNameEventHandler SetClientName;
        public delegate void SetClientNameEventHandler(ServerManager client);

        public event ClientDisconnectedEventHandler ClientDisconnected;
        public delegate void ClientDisconnectedEventHandler(ServerManager client);

        public event ReceivedMessageEventHandler ReceivedMessage;
        public delegate void ReceivedMessageEventHandler(ServerManager client, string message);

        public event ChallengeIssuedEventHandler ChallengeIssued;
        public delegate void ChallengeIssuedEventHandler(ServerManager client, Challenge message);

        public event ChallengeAcceptedEventHandler ChallengeAccepted;
        public delegate void ChallengeAcceptedEventHandler(ServerManager client, Tuple<Challenge, int> message);

        public event ChallengeDeclinedEventHandler ChallengeDeclined;
        public delegate void ChallengeDeclinedEventHandler(ServerManager client, Tuple<Challenge, int> message);

        public ServerManager(TcpListener listen)
        {
            ServerManager.listener = listen;
            serverWorker.WorkerSupportsCancellation = true;
            serverWorker.WorkerReportsProgress = true;
            serverWorker.DoWork += ServerWorker_DoWork;
            serverWorker.ProgressChanged += ServerWorker_ProgressChanged;
            serverWorker.RunWorkerAsync();
        }

        public void SendMessage(string message)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SendMessage(Challenge message)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SendMessage(Tuple<Challenge, int> cResponse)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, cResponse);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SendClientList(List<string> clientNames)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, clientNames);
            }
            catch
            {
                throw;
            }
        }

        private void ServerWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage == 0)
                    NewClientConnected(this);
                else if (e.ProgressPercentage == 1)
                    SetClientName(this);
                else if (e.ProgressPercentage == 2)
                    ReceivedMessage(this, (string)e.UserState);
                else if (e.ProgressPercentage == 3)
                    ChallengeIssued(this, (Challenge)e.UserState);
                else if (e.ProgressPercentage == 4)
                    ChallengeAccepted(this, (Tuple<Challenge, int>)e.UserState);
                else if (e.ProgressPercentage == 5)
                    ChallengeDeclined(this, (Tuple<Challenge, int>)e.UserState);
                else if (e.ProgressPercentage == 9)
                    ClientDisconnected(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ServerWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                // Wait for new connection
                connection = listener.AcceptSocket();

                // Report the connection
                serverWorker.ReportProgress(0);

                // Set up network connection
                socketStream = new NetworkStream(connection);
                reader = new BinaryReader(socketStream);
                writer = new BinaryWriter(socketStream);

                // Loop until connection is lost
                while (!done)
                {
                    try
                    {
                        IFormatter formatter = new BinaryFormatter();
                        object o = (object)formatter.Deserialize(reader.BaseStream);

                        // Check the object type
                        if (o is string)
                        {
                            string cmdCheck = o.ToString()[0].ToString();

                            if (cmdCheck != "!")
                            {
                                latest = (string)o;
                                serverWorker.ReportProgress(2, latest);
                            }
                            else
                            {
                                if (o.ToString().Split("|")[0] == "!name")
                                {
                                    this.name = o.ToString().Split("|")[1];
                                    serverWorker.ReportProgress(1);
                                }
                            }
                        }
                        else if (o is Challenge)
                        {
                            serverWorker.ReportProgress(3, (Challenge)o);
                        }
                        else if (o is Tuple<Challenge, int>)
                        {
                            Tuple<Challenge, int> tup = (Tuple<Challenge, int>)o;

                            if (tup.Item2 == 1)
                                serverWorker.ReportProgress(4, (Tuple<Challenge, int>)o);
                            else if (tup.Item2 == 0)
                                serverWorker.ReportProgress(5, (Tuple<Challenge, int>)o);
                        }
                    }
                    catch (Exception ex)
                    {
                        done = true;
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                serverWorker.ReportProgress(9);
                done = true;
            }
        }
    }
}
