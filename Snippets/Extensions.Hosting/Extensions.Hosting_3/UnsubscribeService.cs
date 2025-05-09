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

// var host = Host.CreateDefaultBuilder(args)
//  .ConfigureServices(services => {
//    services.AddHostedService<UnsubscribeService>();
//  })
//  .Build();
#endregion

public record MyEvent
{
}
