using System;
using System.Collections.Generic;
using System.Text;
using StormLib;

namespace SLUM.lib.Client
{
    public class EventRegistry<T>
    {
        private struct _statusEventData
        {
            public Func<object, T, bool> function;
            public int Priority;
        }
        private LinkedList<_statusEventData> _statusEvents;

        public EventRegistry()
        {
            _statusEvents = new LinkedList<_statusEventData>();
        }
        public void RegisterStatus(int priority, Func<object, T, bool> func)
        {
            if (_statusEvents.Count == 0)
            {
                _statusEvents.AddFirst(new _statusEventData { Priority = priority, function = func });
                return;
            }
            LinkedListNode<_statusEventData> node = _statusEvents.First;
            while (node.Value.Priority <= priority)
                if (node.Next != null)
                    node = node.Next;
            _statusEvents.AddAfter(node, new _statusEventData { Priority = priority, function = func });
        }

        public ExecutionState StatusEvent(object sender, T args)
        {
            LinkedListNode<_statusEventData> node = _statusEvents.First;
            while (node != null)
            {
                if (node.Value.function(sender, args))
                    return ExecutionState.Succeeded();
                node = node.Next;
            }
            return ExecutionState.Failed();
        }
    }
}
