using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ServerBound
{
    public struct LoginPluginResponse : IPacket
    {
        public static int PacketID => 0x02;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarInt)]
        public VarInt MessageID { get; set; }

        [PacketField(1, Types.Boolean)]
        public bool Sucessful { get; set; }

        [PacketField(2, Types.ByteArray)]
        public byte[] Data { get; set; }
    }

}
