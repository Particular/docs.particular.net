using NServiceBus;
using Shared;

class PublisherService(IMessageSession session) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messageId = Guid.NewGuid().ToString();

                Console.WriteLine($"Publishing event {messageId}");
                await session.Publish(new DemoEvent() { Id = messageId }, cancellationToken: cancellationToken);
                await Task.Delay(6000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        }
    }
}