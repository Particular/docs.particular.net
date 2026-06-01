using NServiceBus;

#region Events
public record OrderPlaced(Guid OrderId, string Product) : IEvent;

public record ExpressOrderPlaced(Guid OrderId, string Product) : OrderPlaced(OrderId, Product);
#endregion
