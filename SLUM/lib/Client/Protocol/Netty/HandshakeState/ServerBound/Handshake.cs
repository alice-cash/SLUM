using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.HandshakeState.ServerBound
{
    public struct Handshake : IPacket
    {

        public static int PacketID => 0x00;
        public int GetPacketID => PacketID;

        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarInt)]
        public VarInt ProtocolVersion { get; set; }
        [PacketField(1, Types.VarString, 255)] 
        public VarString ServerAddress { get; set; }
        [PacketField(2, Types.UShort)]
        public ushort ServerPort { get; set; }
        [PacketField(3, Types.EnumNextState)]
        public NextState NextState { get; set; }

    }
}
