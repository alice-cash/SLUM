using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty736
{
    /// <summary>
    /// Protocol Handling for Release 1.16.1
    /// </summary>
    class Protocol : IProtocol
    {
        public uint Version => 736;

        public NetworkProtocol Format => NetworkProtocol.JavaNetty;

    }
}
