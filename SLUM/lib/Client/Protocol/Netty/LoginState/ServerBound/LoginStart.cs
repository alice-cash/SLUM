using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ServerBound
{
    public struct LoginStart : IPacket
    {
        public static int PacketID => 0x00;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarString, 16)]
        public VarString Name { get; set; }
    }

}

