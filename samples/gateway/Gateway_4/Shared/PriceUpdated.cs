using System;
using NServiceBus;

namespace Shared
{
    public class PriceUpdated :
        IMessage
    {
        public int ProductId { get; set; }
        public double NewPrice { get; set; }
        public DateTime ValidFrom { get; set; }
    }
}