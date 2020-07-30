using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client.Protocol.Netty
{
    public enum NextState: int
    {
        Handshake = 0,
        Status = 1,
        Login = 2
    }
}
