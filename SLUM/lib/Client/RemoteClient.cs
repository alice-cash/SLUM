using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using SLUM.lib.Client.Protocol;
//using SLUM.lib.Client.Protocol.Netty;
using SLUM.lib.Data;

namespace SLUM.lib.Client
{
    public class RemoteClient
    {

        public IProtocol ClientProtocol { get; internal set; }
        public INetConnection ClientConnection { get; internal set; }

        public EventHandler DisconnectedEvent;

        public ByteReader StreamReader;
        public ByteWriter StreamWriter;

        public ClientData Data;

        private ClientState _state;
        private Action TickMethod;

        public ClientState State
        {
            get { return _state; }
            set
            {
                var oldState = _state;
                _state = value;
                DoStateChange(oldState, value);
            }
        }

        public EventHandler<StateChangeEventArgs> StateChange;

        private void DoStateChange(ClientState oldState, ClientState newState)
        {
            var e = new StateChangeEventArgs() { PreviousState = oldState, NewState = newState };
            switch (newState)
            {
                case ClientState.Handshaking:
                    TickMethod = TickHandshaking;
                    break;
                case ClientState.Login:
                    TickMethod = TickLogin;
                    break;
                case ClientState.Play:
                    TickMethod = TickPlay;
                    break;
                case ClientState.Status:
                    TickMethod = TickStatus;
                    break;
            }
            StateChange?.Invoke(this, e);
        }
        public RemoteClient(INetConnection netConnection)
        {
            ClientConnection = netConnection;
            ClientProtocol = new Protocol.UnknownProtocol(this);
            StreamReader = new ByteReader(netConnection);
            StreamWriter = new ByteWriter(netConnection);
            State = ClientState.Handshaking;
        }

        public IPAddress RemoteAddress
        {
            get
            {
                return ClientConnection.RemoteAddress;
            }
        }

        public bool IsConnected { get { return ClientConnection.IsConnected; } }

        internal void Poll()
        {
            TickMethod();

            if (ClientConnection.DataAvaliable)
                ClientProtocol.ReadClient();
        }

        private void TickHandshaking()
        {

        }
        private void TickStatus()
        {
            if(Data.LastLogicState != ClientState.Status)
            {
                Data.LastLogicState = ClientState.Status;
                Data.StatusRequest = false;
                Data.StatusRespond = false;
                Data.StatusPing = false;
            } 

            ClientProtocol.TickStatus();

        }

        private void TickLogin()
        {
            if (Data.LastLogicState != ClientState.Status)
            {
                Data.LastLogicState = ClientState.Status;
                Data.LoginStart = false;
                Data.LoginEncryptionResponseRequired = false;
                Data.LoginRespondRequired = false;
                Data.LoginSecret = new byte[0];
                Data.LoginVerifyToken = new byte[0];
                Data.Token = new byte[0];

            }

            ClientProtocol.TickLogin();
        }

        private void TickPlay()
        {

        }

        internal void Disconnect()
        {
            if (IsConnected)
                ClientConnection.Disconnect();
        }
        /*
        internal void SendPacket(IPacket packet)
        {
            packet.
            ClientConnection.SendBytes(packet.GeneratePacketBytes());
        }*/
    }
}
