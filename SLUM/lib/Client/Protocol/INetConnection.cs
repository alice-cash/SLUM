using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol
{
    public interface INetConnection
    {
        void SendPacket(byte[] data);
    }
}
