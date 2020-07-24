using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol
{
    public enum ClientState
    {
        Handshaking,
        Status,
        Login,
        Play,
        Any
    }
}
