using SLUM.lib.Client.Events;

namespace SLUM.lib.Client
{
    public static class EventManager
    {
        public static EventRegistry<StatusEventArgs> StatusEvent = new EventRegistry<StatusEventArgs>();

    }
}
