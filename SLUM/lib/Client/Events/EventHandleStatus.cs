using System;
using System.Collections.Generic;
using System.Text;

namespace SLUM.lib.Client.Events
{
    public enum EventHandleStatus
    {
        NotHandled,
        Cancel,
        Continue,
        ReadOnly,
    }
}
