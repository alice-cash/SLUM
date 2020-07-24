using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound
{
    public class StatusPong : Packet
    {
        public static new int PacketID => 0x01;
        public StatusPong()
        {

        }
        public override void TryReadStream(RemoteClient client)
        {
            throw new NotImplementedException();
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            client.StreamWriter.Write(client.Data.StatusPingPayload, false);

        }
    }
}
