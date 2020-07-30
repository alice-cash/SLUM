using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ClientBound
{
    public struct LoginSetCompression : IPacket
    {
        public static int PacketID => 0x03;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarInt)]
        public VarInt SizeThreashold { get; set; }
    }


}
