using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.HandshakeState.ServerBound
{
    public class Handshake : Packet
    {
        public Int32 ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        public NextState NextState;

        public static new int PacketID => 0x00;

        public Handshake(RemoteClient client) : base(client)
        {
        }

        public override void TryReadStream(RemoteClient client)
        {
            PacketGood = _TryRead(client);
        }

        private bool _TryRead(RemoteClient client)
        {

            var readVarInt = client.StreamReader.ReadVarInt();
            if (!readVarInt) return false;
            ProtocolVersion = readVarInt.Result;

            var readVarString = client.StreamReader.ReadVarString();
            if (!readVarString) return false;
            ServerAddress = readVarString.Result;

            var readUshort = client.StreamReader.ReadUShort();
            if (!readVarInt) return false;
            ServerPort = readUshort.Result;
            
            readVarInt = client.StreamReader.ReadVarInt();
            if (!readVarInt) return false;
            if (Enum.IsDefined(typeof(NextState), readVarInt.Result))
                NextState = (NextState)readVarInt.Result;
            else return false;

            return true;
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            throw new NotImplementedException();
        }
    }
}
