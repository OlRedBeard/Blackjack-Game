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

        // String for latest message
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

        private void ServerWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage == 0)
                    NewClientConnected(this);
                else if (e.ProgressPercentage == 1)
                    SetClientName(this);
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
