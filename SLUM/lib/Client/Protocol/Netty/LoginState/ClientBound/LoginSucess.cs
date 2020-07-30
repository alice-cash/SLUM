using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.LoginState.ClientBound
{
    public struct LoginSucess : IPacket
    {
        public static int PacketID => 0x02;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.VarString)]
        public VarString UUID { get; set; }
        [PacketField(1, Types.VarString)]
        public VarString Username { get; set; }
    }

}
