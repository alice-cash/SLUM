using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Alpha51
{
    class Protocol : IProtocol
    {
        public uint Version => 51;

        public NetworkProtocol Format => NetworkProtocol.JavaAlpha;
    }
}
