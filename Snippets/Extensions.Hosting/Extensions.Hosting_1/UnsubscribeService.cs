using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region UnsubscribeBackgroundService
public sealed class UnsubscribeService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await messageSession.Unsubscribe<MyEvent>();
    }
}
#endregion

public record MyEvent
{
}
