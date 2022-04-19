using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackClasses
{
    public class GameServer
    {
        public static TcpListener listener;
        private BackgroundWorker bgw = new BackgroundWorker();
        private Socket socket;
        private NetworkStream socketStream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public delegate void OpponenentConnectedEventHandler(GameServer client);
        public event OpponenentConnectedEventHandler OpponenentConnected;

        public delegate void OpponenentUpdateEventHandler(Player opponent);
        public event OpponenentUpdateEventHandler OpponentUpdate;

        bool done = false;

        public GameServer(TcpListener listener)
        {
            GameServer.listener = listener;
            bgw.WorkerSupportsCancellation = true;
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += Bgw_DoWork;
            bgw.ProgressChanged += Bgw_ProgressChanged;
            bgw.RunWorkerAsync();
        }

        public void SendCardInfo(Tuple<Card,int> details)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, details);
            }
            catch
            {
                throw;
            }
        }

        public void EndTurn()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, 1);
            }
            catch
            {
                throw;
            }
        }

        public void SendPlayerInfo(Player p)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer.BaseStream, p);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Bgw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            try
            {
                // Client connected
                if (e.ProgressPercentage == 0)
                    OpponenentConnected(this);
                // Client updated
                else if (e.ProgressPercentage == 1)
                    OpponentUpdate((Player)e.UserState);
            }
            catch (Exception ex)
            {

            }
        }

        private void Bgw_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                socket = listener.AcceptSocket();
                bgw.ReportProgress(0); // Flag for connected client

                socketStream = new NetworkStream(socket);
                reader = new BinaryReader(socketStream);
                writer = new BinaryWriter(socketStream);

                while (!done)
                {
                    try
                    {
                        IFormatter formatter = new BinaryFormatter();
                        object o = (object)formatter.Deserialize(reader.BaseStream);

                        if (o is Player)
                        {
                            bgw.ReportProgress(1, (Player)o);
                        }
                    }
                    catch
                    {
                        done = true;
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
