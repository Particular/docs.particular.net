using Microsoft.Extensions.Hosting;

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