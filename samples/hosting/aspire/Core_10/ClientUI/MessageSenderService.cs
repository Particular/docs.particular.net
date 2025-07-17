using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClientUI;

class MessageSenderService(IMessageSession messageSession, ILogger<MessageSenderService> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var i = 1;

        while (!stoppingToken.IsCancellationRequested)
        {
            await messageSession.Send(new PlaceOrder { OrderId = Guid.NewGuid().ToString() }, cancellationToken: stoppingToken);
            log.LogInformation("Sent a message");

            await Task.Delay(i++ * 500, stoppingToken);
        }
    }
}