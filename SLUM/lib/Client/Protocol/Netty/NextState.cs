using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol.Netty
{
    public enum NextState
    {
        Handshake = 0,
        Status = 1,
        Login = 2
    }
}
