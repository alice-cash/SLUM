using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client.Protocol;

namespace SLUM.lib.Client
{
    public struct ClientData
    {
        public int TimeoutCounter;
        public ClientState LastLogicState;
        public bool Compression;


        public bool StatusRequest;
        public bool StatusRespond;
        public bool StatusPing;
        public long StatusPingPayload;



    }
}
