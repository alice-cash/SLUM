using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SLUM.lib.Config;
using StormLib.Threading;

namespace SLUM.lib.Client
{
    public class TCPListener : IThreadTask
    {

        public static TCPListener _instance;
        public static TCPListener Instance { get { if (_instance == null) CreateInstance(); return _instance; } }

        public static void CreateInstance()
        {
            if (_instance != null)
                return;
            _instance = new TCPListener();
        }

        private Thread _listenThread;
        private bool _threadRunning;
        private TcpListener _listiner;
        private TcpListener _secondaryListiner;
        private bool _secondaryListinerActive;

        private List<RemoteClient> _clients;

        private TCPListener()
        {
            _clients = new List<RemoteClient>();
            _threadRunning = true;

            _listenThread = new Thread("Client Listen Thread");
            _listenThread.AddTask(this);
            _secondaryListinerActive = false;
            _listenThread.Start();
            _instance = this;
        }

        private void AcceptConnection(Socket client)
        {
            TCPConnection TCPConnection = new TCPConnection(client);
            RemoteClient Conn = new RemoteClient(TCPConnection);
            _clients.Add(Conn);
            Trace.WriteLine(string.Format("Connection Received from '{0}'.", Conn.RemoteAddress.ToString()));
            Conn.DisconnectedEvent += delegate (object sender, EventArgs e)
            {
                StormLib.Console.WriteLine("Client Disconnected.");
                //If its not a sender item, then it will be null
                //and a null should "NOT" be in the clients list
                if (_clients.Contains(sender as RemoteClient))
                    _clients.Remove(sender as RemoteClient);
            };
        }

        public void RunTask()
        {
            if (_listiner.Pending())
            {
                AcceptConnection(_listiner.AcceptSocket());
            }

            if (_secondaryListinerActive && _secondaryListiner.Pending())
            {
                AcceptConnection(_secondaryListiner.AcceptSocket());
            }

            for (int i = 0; i < _clients.Count; i++)
            {
                if(_clients[i].IsConnected)
                    _clients[i].Poll();
            }
        }

        public void Start()
        {
            //If the Listen address is IPv6Any, then we possibly need to create a second listener for IPv4
            if (ServerConfig.Instance.ConvertedClientListenAddress == IPAddress.IPv6Any)
            {
                _secondaryListinerActive = true;
                _secondaryListiner = new TcpListener(IPAddress.Any, ServerConfig.Instance.ClientListenPort);
                _secondaryListiner.Start();
            }
            _listiner = new TcpListener(ServerConfig.Instance.ConvertedClientListenAddress, ServerConfig.Instance.ClientListenPort);
            _listiner.Start();
        }


        public void Stop(bool force = false)
        {
            _listiner.Stop();
            if (_secondaryListinerActive) _secondaryListiner.Stop();
            _listenThread.Stop();
        }
    }
}
