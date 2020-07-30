using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound
{
    public struct StatusResponse : IPacket
    {
        public static int PacketID => 0x00;
        public int GetPacketID => PacketID;

        [PacketField(0, Types.Chat, 32767)]
        public Chat StatusData { get; set; }

        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }
    }
}
