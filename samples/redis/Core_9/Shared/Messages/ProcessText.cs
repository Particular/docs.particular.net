using NServiceBus;

namespace Shared.Messages;

public class ProcessText : ICommand
{
    public string RedisKey { get; set; }
}
