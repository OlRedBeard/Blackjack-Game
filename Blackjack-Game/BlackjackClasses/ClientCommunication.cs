﻿using System;
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

        public ClientCommunication(string serv)
        {
            this.serverName = serv;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
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
