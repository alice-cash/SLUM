using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty
{
    public abstract class Protocol : IProtocol
    {
        public abstract uint Version { get; }
        protected RemoteClient _client;
        public NetworkProtocol Format => NetworkProtocol.JavaNetty;

        public Protocol(RemoteClient client)
        {
            _client = client;
            client.StateChange += (o, e) => SetupAllowedPacket();
        }

        protected abstract void SetupAllowedPacket();


        public void ReadClient()
        {
            if (_client.ClientConnection.DataAvaliable)
            {
                var Length = _client.StreamReader.ReadVarInt();
                var PacketID = _client.StreamReader.ReadVarInt();
                Packet packet = null;

                if (Length.Sucess && PacketID.Sucess)
                {
                    if(_client.State == ClientState.Status)
                    {
                        // Handshake is taken care of in the UnknownProtocol class

                        switch (PacketID.Result)
                        {
                            case 0x00:
                                packet = new StatusRequest();
                                break;
                            case 0x01:
                                packet = new StatusPing();
                                break;
                        }
                        
                        
                    }

                }

                if (packet == null)
                {
                    Trace.WriteLine("Packet recieved: " + Length.Result);
                    // _client.Disconnect();
                }
                else
                {
                    packet.TryReadStream(_client);
                    HandlePacket(packet);
                }

            }
        }

        public bool HandlePacket(Packet packet)
        {
            if (packet.PacketGood == false)
            {
                _client.Disconnect();
                return false;
            }
            if (packet is Handshake)
            {
                if (((Handshake)packet).ProtocolVersion != Version)
                {
                    Trace.WriteLine(string.Format("Protocol versiom mismatch! Check handling for {0}", this.GetType().ToString()));
                    _client.Disconnect();
                    return false;
                }
                if (((Handshake)packet).NextState == NextState.Status)
                    _client.State = ClientState.Status;
                else if (((Handshake)packet).NextState == NextState.Login)
                    _client.State = ClientState.Login;
                else
                {
                    _client.Disconnect();
                    return false;
                }
                return true;
            }

            if (packet is StatusRequest)
            {
                _client.Data.StatusRequest = true;
                _client.Data.StatusRespond = false;
                return true;
            }

            if (packet is StatusPing)
            {
                _client.Data.StatusPing = true;
                _client.Data.StatusPingPayload = (packet as StatusPing).StatusPingPayload;
            }
            return _handlePacket(packet);
        }

        protected internal abstract bool _handlePacket(Packet packet);

        public void TickStatus()
        {
            if (_client.Data.StatusRequest && !_client.Data.StatusRespond)
            {
                StatusResponse packet = new StatusResponse();
                packet.SendPacket(_client);
                _client.Data.StatusRespond = true;
                return;
            }
            if (_client.Data.StatusPing)
            {
                var packet = new StatusPong();
                packet.SendPacket(_client);
                _client.Disconnect();
            }
        }
    }
}
