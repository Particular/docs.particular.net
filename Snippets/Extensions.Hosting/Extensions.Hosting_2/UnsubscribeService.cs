using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region UnsubscribeBackgroundService
public sealed class UnsubscribeService : BackgroundService
{
    readonly IMessageSession _messageSession;

    public UnsubscribeService(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageSession.Unsubscribe<MyEvent>();
    }
}
#endregion

public record MyEvent
{
}
