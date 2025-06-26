using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.events
{
    public class Event
    {
        public int eventCode;

        public object value;


        public int getEventCode() {
            return eventCode;
        }

        public object getValue() {
            return value;
        }

        public Event(int ec, object val) {

            this.eventCode = ec;
            this.value = val;

        }

    }
}
