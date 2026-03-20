using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public record OrderPlaced(Guid OrderId, string Product, int Quantity) : IEvent
    {
    }
}
