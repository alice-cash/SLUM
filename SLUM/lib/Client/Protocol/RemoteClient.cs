using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol
{
    public class RemoteClient
    {
        private IProtocol ClientProtocl;
        private INetConnection ClientConnection;

        public RemoteClient(INetConnection netConnection)
        {
            ClientConnection = netConnection;
        }

        public void SendPacket(byte[] data)
        {
            ClientConnection.SendPacket(data);
        }
    }
}
