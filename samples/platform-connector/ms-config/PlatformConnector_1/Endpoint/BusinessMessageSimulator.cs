using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class BusinessMessageSimulator : BackgroundService
{
    IMessageSession messageSession;

    public BusinessMessageSimulator(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await messageSession.SendLocal(new BusinessMessage {BusinessId = Guid.NewGuid()});
                await Task.Delay(200, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {

        }
    }
}