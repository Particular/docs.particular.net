using NServiceBus;

namespace Events;

public interface IOrderPackaged : IEvent
{
    Guid CustomerId { get; set; }
    string OrderId { get; set; }
}