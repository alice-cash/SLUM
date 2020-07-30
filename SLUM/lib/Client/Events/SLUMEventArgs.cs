using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Events
{
    public class SLUMEventArgs : EventArgs
    {
        public EventHandleStatus Handeled {get;set;}
        public RemoteClient SendingClient { get; set; }
    }
}
