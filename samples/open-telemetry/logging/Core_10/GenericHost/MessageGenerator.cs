class MessageGenerator(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var number = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                await messageSession.SendLocal(new MyMessage { Number = number++ }, cancellationToken);

                await Task.Delay(1000, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}