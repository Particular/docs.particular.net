using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public record OrderAccepted(Guid orderId) : IEvent
    {
    }
}
