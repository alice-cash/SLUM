using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol
{
    public interface IProtocol
    {
        /// <summary>
        /// Return the Protocol Version
        /// </summary>
        public uint Version { get; }

        public NetworkProtocol Format { get; }
    }
}
