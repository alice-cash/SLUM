using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ServerBound
{
    public struct StatusRequest : IPacket
    {
        public static int PacketID => 0x00;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }
    }
}
