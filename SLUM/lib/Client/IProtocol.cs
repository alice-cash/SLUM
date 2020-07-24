using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client.Protocol;

namespace SLUM.lib.Client
{
    public interface IProtocol
    {
        /// <summary>
        /// Return the Protocol Version
        /// </summary>
        public uint Version { get; }

        public NetworkProtocol Format { get; }

        void ReadClient();
        void TickStatus();
    }
}
