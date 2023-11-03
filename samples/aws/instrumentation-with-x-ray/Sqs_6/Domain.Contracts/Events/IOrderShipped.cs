using NServiceBus;

namespace Events;

public interface IOrderShipped : IEvent
{
    Guid CustomerId { get; set; }
    Guid OrderId { get; set; }
}