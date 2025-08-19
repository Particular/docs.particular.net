using NServiceBus;

namespace ServerShared;

public record CompleteOrder : ICommand
{
    public string OrderDescription { get; set; }
}