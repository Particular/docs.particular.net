using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Shared;

class MessageSender(ILogger<MessageSender> logger, IMessageSession messageSession) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();

        var message = new RequestMessage
        {
            Id = guid,
            Data = "String property value"
        };

        await messageSession.Send(message, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation($"Message sent, requesting to get data by id: {guid:N}");
        logger.LogInformation("Use 'docker-compose down' to stop containers.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    readonly ILogger logger = logger;
}
