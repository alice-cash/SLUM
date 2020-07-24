using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ServerBound
{
    public class StatusPing : Packet
    {
        public static new int PacketID => 0x00;

        public long StatusPingPayload { get; private set; }

        public StatusPing()
        {
            PacketGood = false;
        }
        public override void TryReadStream(RemoteClient client)
        {
            var readVarLong = client.StreamReader.ReadLong();
            if (!readVarLong) { client.Disconnect(); return; }
            StatusPingPayload = readVarLong.Result;
            PacketGood = true;
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            throw new NotImplementedException();
        }
    }
}
