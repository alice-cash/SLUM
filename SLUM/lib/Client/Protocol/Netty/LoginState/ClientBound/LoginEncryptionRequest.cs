using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ClientBound
{
    public struct LoginEncryptionRequest : IPacket
    {
        public static int PacketID => 0x01;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Data.DataTypes.Types.VarString)]
        public String ServerID { get; set; }

        [PacketField(1, Data.DataTypes.Types.ByteArray)]
        public ByteArray PublicKey { get; set; }

        [PacketField(2, Data.DataTypes.Types.ByteArray)]
        public ByteArray VerifyToken { get; set; }

    }
}
