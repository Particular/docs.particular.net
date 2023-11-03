using NServiceBus;

namespace Events;

public interface IStockUpdated : IEvent
{
    string ProductId { get; set; }
}