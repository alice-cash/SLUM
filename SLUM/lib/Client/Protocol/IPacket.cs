using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;

namespace SLUM.lib.Client.Protocol
{
    public interface IPacket
    {
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }
        public static int PacketID { get; }
        public int GetPacketID { get; }

    }
}
