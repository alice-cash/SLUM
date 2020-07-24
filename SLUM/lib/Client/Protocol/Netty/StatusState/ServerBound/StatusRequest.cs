using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ServerBound
{
    public class StatusRequest : Packet
    {
        public static new int PacketID => 0x00;
        public StatusRequest()
        {
            PacketGood = false;
        }
        public override void TryReadStream(RemoteClient client)
        {
            PacketGood = true;
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            throw new NotImplementedException();
        }
    }
}
