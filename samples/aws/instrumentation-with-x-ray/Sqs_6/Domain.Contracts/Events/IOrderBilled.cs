using NServiceBus;

namespace Events;

public interface IOrderBilled : IEvent
{
    Guid CustomerId { get; set; }
    Guid OrderId { get; set; }
}