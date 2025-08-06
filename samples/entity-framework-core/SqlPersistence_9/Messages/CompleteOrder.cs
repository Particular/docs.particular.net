using NServiceBus;

public class CompleteOrder : ICommand
{
    public string OrderId { get; set; }
}