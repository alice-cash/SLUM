using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client.Protocol.Netty;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol
{
    /// <summary>
    /// Class for when the remote protocol is unknown and we need to find and send it off to the right class.
    /// </summary>
    public class UnknownProtocol : IProtocol
    {
        public uint Version => 0;

        public NetworkProtocol Format => NetworkProtocol.Unknown;
        public RemoteClient Client { get; protected set; }

        public UnknownProtocol(RemoteClient client)
        {
            Client = client;
        }
        public void ReadClient()
        {
            if (Client.ClientConnection.BytesAvaliable >= 1)
            {
                // Alpha netcode
                // -> 0x02 - 0x## - 
                // a1.2.0_02
                // 0x02 0x00 0x09 0x50 0x6c 0x61 0x79 0x65 0x72 0x31 0x33 0x34
                // r1.2.5
                // 0x02 0x00 0x18 0x00 0x41 0x00 0x6c 0x00 0x69 0x00 0x63 0x00 0x65 0x00 0x33 0x00 0x36 0x00 0x35 0x00 0x3b 0x00 0x31 0x00 0x32 0x00 0x37 0x00 0x2e 0x00 0x30 0x00 0x2e 0x00 0x30 0x00 0x2e 0x00 0x31 0x00 0x3a 0x00 0x32 0x00 0x35 0x00 0x35 0x00 0x36 0x00 0x35

                // Netty 
                // r1.16.1
                // 0x10 0x00 0xe0 0x05 0x09 0x31 0x32 0x37 0x2e 0x30 0x2e 0x30 0x2e 0x31 0x63 0xdd 0x02 0x0a 0x00 0x08 0x41 0x6c 0x69 0x63 0x65 0x33 0x36 0x35

                // Bedrock should be 0x01

                // Given the above, we can do a dumb "If byte 0 == 0x02 then legacy, if 0x01 then Bedrock, otherwise netty"

                var data = Client.StreamReader.ReadByte(true);
                if (!data)
                {
                    Client.Disconnect();
                    return;
                }

                if (data.Result == 0x02)
                {
                    Disconnect("Client not supported", NetworkProtocol.JavaAlpha);
                    return;
                }

                if (data.Result == 0xFE)
                {
                    Disconnect("Client not supported", NetworkProtocol.JavaAlpha);
                    return;
                }

                if (data.Result == 0x01)
                {
                    Disconnect("Client not supported", NetworkProtocol.BedrockRaknet);
                    return;
                }

                //Try Netty

                TryReadNettyHandShake();
            }
        }

        private void TryReadNettyHandShake()
        {
            var Length = Client.StreamReader.ReadVarInt();
            var PacketID = Client.StreamReader.ReadVarInt();

            if (Length.Sucess && PacketID.Sucess && PacketID.Result == 0x00)
            {
                // Handshake packet 0x00 
                Handshake handshake = new Handshake(Client);
                if (handshake.PacketGood)
                {
                    // We try and setup the correct protocol class then 
                    switch (handshake.ProtocolVersion)
                    {
                        case 736:
                            Client.ClientProtocol = new Netty736.Protocol(Client);
                            break;

                        default:
                            Client.Disconnect();
                            return;
                    }

                    ((Netty.Protocol)Client.ClientProtocol).HandlePacket(handshake);
                    return;
                }

            }

            // Somthing went wrong, we will just close out.
            Client.Disconnect();
        }

        private void Disconnect(string Reason, NetworkProtocol protocol)
        {
            switch (protocol)
            {
                case NetworkProtocol.JavaAlpha:

                    break;
                case NetworkProtocol.JavaNetty:
                    Packet Disconnect = new Disconnect(Reason);
                    Disconnect.SendPacket(Client);
                    break;

                case NetworkProtocol.BedrockRaknet:

                    break;
            }

            Client.Disconnect();
        }

        public void TickStatus()
        {
            throw new NotImplementedException();
        }
    }
}
