using NServiceBus;

namespace Shared;

public class AddItem : ICommand
{
    public Guid OrderId { get; set; }
    
    public string Name { get; set; }
}
