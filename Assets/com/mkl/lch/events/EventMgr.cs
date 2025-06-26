using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;


namespace com.mkl.lch.events
{
    public class EventMgr
    {
        private readonly ConcurrentQueue<Event> _queue = new ConcurrentQueue<Event>();

        public void Enqueue(Event item)
        {
            _queue.Enqueue(item);
            
        }

        public bool HasItems()
        {
            return !_queue.IsEmpty;
        }

        public Event Acquire()
        {
            return _queue.TryDequeue(out var ev) ? ev : null;
        }
    }
}
