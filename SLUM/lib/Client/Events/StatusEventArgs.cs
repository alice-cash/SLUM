using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Events
{
    public class StatusEventArgs : SLUMEventArgs
    {
        public StatusEventArgs(RemoteClient client)
        {
            SendingClient = client;
        }
    }
}
