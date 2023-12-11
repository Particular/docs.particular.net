using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

class PublisherService : BackgroundService
{
    private readonly IMessageSession messageSession;
    
    public PublisherService(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var messageId = Guid.NewGuid().ToString();

            Console.WriteLine($"Publishing event {messageId}");
            await messageSession.Publish(new DemoEvent() { Id = messageId });
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}