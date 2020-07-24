using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data;

namespace SLUM.lib.Client.Protocol
{
    public interface IPacket
    {

        public void SendPacket(RemoteClient client);

    }
}
