using NServiceBus;

namespace Shared.Messages;

public class ProcessText : ICommand
{
    public string LargeText { get; set; }
}
