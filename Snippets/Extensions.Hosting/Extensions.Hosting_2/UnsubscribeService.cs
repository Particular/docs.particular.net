using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region UnsubscribeBackgroundService
public sealed class UnsubscribeService : BackgroundService
{
    IMessageSession _messageSession;

    public UnsubscribeService(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageSession.Unsubscribe<MyEvent>();
    }
}

// var host = Host.CreateDefaultBuilder(args)
//  .ConfigureServices(services => {
//    services.AddHostedService<UnsubscribeService>();
//  })
//  .Build();
#endregion

public record MyEvent
{
}
