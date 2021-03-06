﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound
{
    public struct StatusPong : IPacket
    {
        public static int PacketID => 0x01;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Data.DataTypes.Types.Long)]
        public long PingPayload { get; set; }
    }

}
