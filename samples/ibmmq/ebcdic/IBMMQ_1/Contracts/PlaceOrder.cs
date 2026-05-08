using NServiceBus;

public record PlaceOrder(Guid OrderId, string Product, int Quantity) : ICommand;
