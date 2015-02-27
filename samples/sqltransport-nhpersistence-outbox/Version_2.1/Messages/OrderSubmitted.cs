using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class OrderSubmitted : IEvent
    {
        public string OrderId { get; set; }
        public decimal Value { get; set; }
    }
}
