using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ClientBound
{
    public struct LoginPluginRequest : IPacket
    {

        public static int PacketID => 0x04;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarInt)]
        public int MessageID { get; set; }
        [PacketField(1, Types.Identifier)]
        public Identifier Channel { get; set; }
        [PacketField(2, Types.ByteArray, -1, false)]
        public byte[] Data { get; set; }
    }

}
