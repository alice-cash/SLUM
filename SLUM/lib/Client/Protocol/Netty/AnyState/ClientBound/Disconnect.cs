using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty.AnyState.ClientBound
{
    public class Disconnect : Packet
    {
        String Reason { get; set; }

        public static new int PacketID => 0x00;

        public Disconnect()
        {
        }

        public Disconnect(string Text)
        {
            Reason = Text;
        }

        internal override void GeneratePacketData(RemoteClient client)
        {
            client.StreamWriter.Write(Reason, true);
            System.Diagnostics.Trace.WriteLine("Disconnect - " + Reason);

        }

        public override void TryReadStream(RemoteClient client)
        {
            throw new NotImplementedException();
        }
    }
}
