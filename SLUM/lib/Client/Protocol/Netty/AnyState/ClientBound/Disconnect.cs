﻿using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.AnyState.ClientBound
{
    public struct Disconnect : IPacket
    {
        public static int PacketID => 0x00;
        public int GetPacketID => PacketID;
        public bool PacketGood { get; set; }
        public int PacketLength { get; set; }

        [PacketField(0, Types.Chat)]
        public Chat Reason { get; set; }

    }
}
