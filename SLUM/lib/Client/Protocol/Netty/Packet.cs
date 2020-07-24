using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty
{
    public abstract class Packet : IPacket
    {
        public bool PacketGood { get; set; }
        //public bool Compression { get; set; }

        public static int PacketID { get; }

        public Packet()
        {
        }

        public Packet(RemoteClient client)
        {
            TryReadStream(client);
        }

        public void SendPacket(RemoteClient client)
        {

            client.StreamWriter.Write(PacketID, true);
            GeneratePacketData(client);
            Int32 length = client.StreamWriter.BufferLength;

            if (client.Data.Compression)
            {
                client.StreamWriter.GZipEncode();
                client.StreamWriter.Write(length, true, true);
            }
            length = client.StreamWriter.BufferLength;
            client.StreamWriter.Write(length, true, true);
            client.StreamWriter.Flush();
        }

        public abstract void TryReadStream(RemoteClient client);

        internal abstract void GeneratePacketData(RemoteClient client);
    }
}
