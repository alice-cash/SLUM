using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Protocol
{
    public class StateChangeEventArgs: EventArgs
    {
        public ClientState NewState { get; set; }
        public ClientState PreviousState { get; set; }
    }
}
