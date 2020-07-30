using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client.Protocol;
using SLUM.lib.Data.DataTypes;

namespace SLUM.lib.Client
{
    public struct ClientData
    {
        public int TimeoutCounter;
        public ClientState LastLogicState;
        public bool Compression;

        public bool Encrypted;


        public bool StatusRequest;
        public bool StatusRespond;
        public bool StatusPing;
        public long StatusPingPayload;

        internal bool LoginStart;
        internal bool LoginEncryptionResponseRequired;
        internal byte[] LoginSecret;
        internal byte[] LoginVerifyToken;
        internal byte[] Token;
        internal VarString Username;
        internal bool LoginRespondRequired;
        internal string UUID;
    }
}
