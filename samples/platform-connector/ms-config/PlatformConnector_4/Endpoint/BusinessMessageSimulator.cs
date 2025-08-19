using Microsoft.Extensions.Hosting;

class BusinessMessageSimulator(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await messageSession.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                Console.WriteLine("Message sent");
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}