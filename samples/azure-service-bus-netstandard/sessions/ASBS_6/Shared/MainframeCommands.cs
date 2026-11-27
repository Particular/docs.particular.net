using NServiceBus;

namespace Shared;

public class BeginTransaction : ICommand
{
}

public class Reserve : ICommand
{
    public Guid OrderId { get; set; }
}
public class CompleteTransaction : ICommand
{
}