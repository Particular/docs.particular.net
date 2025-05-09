using System;
using Microsoft.Extensions.Hosting;

#region UnsubscribeBackgroundService
public class UnsubscribeService(IMessageSession messageSession) : BackgroundService
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