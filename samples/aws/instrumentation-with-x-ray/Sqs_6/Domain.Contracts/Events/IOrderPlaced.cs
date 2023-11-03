using NServiceBus;

namespace Events;

public interface IOrderPlaced : IEvent
{
    public Guid CustomerId { get; set; }
    public OrderDetails Order { get; set; }
}