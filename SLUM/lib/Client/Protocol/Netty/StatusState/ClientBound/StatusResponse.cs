using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty.StatusState.ClientBound
{
    public class StatusResponse : Packet
    {
        public static new int PacketID => 0x01;

        public StatusResponse()
        {

        }
        public override void TryReadStream(RemoteClient client)
        {
            throw new NotImplementedException();
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            string RESPONSE = "{\"version\":{\"name\":\"Wak, 1.16.x\",\"protocol\":736},\"players\":{\"max\":300,\"online\":0,\"sample\":[{\"name\":\"Wak\"" +
                ",\"id\":\"00000000-0000-0000-0000-000000000000\"}" +
                "]},\"description\":{\"extra\":[{\"color\":\"white\",\"text\":\"grief survival!\\n\"},{\"color\":\"dark_purple\",\"text\":\"DRAGONS!\"}],\"text\":\"\"}}";
            client.StreamWriter.Write(RESPONSE, true);
        }
    }
}
