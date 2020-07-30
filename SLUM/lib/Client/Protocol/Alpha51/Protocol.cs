using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Alpha51
{
    public class Protocol : IProtocol
    {
        public uint Version => 51;
        private RemoteClient _client;

        public NetworkProtocol Format => NetworkProtocol.JavaAlpha;

        uint IProtocol.Version => throw new NotImplementedException();

        NetworkProtocol IProtocol.Format => throw new NotImplementedException();

        public void ReadClient()
        {
            throw new NotImplementedException();
        }

        public void TickStatus()
        {
            throw new NotImplementedException();
        }

        public void TickLogin()
        {
            throw new NotImplementedException();
        }
    }
}
