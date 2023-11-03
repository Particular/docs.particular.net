using NServiceBus;

namespace Events;

public interface IOrderDelivered : IEvent
{
    Guid CustomerId { get; set; }
    string OrderId { get; set; }
}