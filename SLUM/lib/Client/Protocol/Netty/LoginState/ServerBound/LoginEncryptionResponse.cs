using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ServerBound
{
    public struct LoginEncryptionResponse : IPacket
    {
        public static int PacketID => 0x01;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.ByteArray)]
        public byte[] Secret { get; set; }

        [PacketField(1, Types.ByteArray)]
        public byte[] Token { get; set; }
    }

}
