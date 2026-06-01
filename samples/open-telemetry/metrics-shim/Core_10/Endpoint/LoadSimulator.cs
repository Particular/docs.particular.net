// Simulates busy (almost no delay) / quite time in a sine wave
class LoadSimulator(IMessageSession messageSession, TimeSpan minimumDelay, TimeSpan idleDuration)
{

    public void Start()
    {
        cancellation = new CancellationTokenSource();
        _ = Task.Run(() => Loop(cancellation.Token));
    }

    async Task Loop(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Work(cancellationToken);
                var delay = NextDelay();
                await Task.Delay(delay, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    TimeSpan NextDelay()
    {
        var angleInRadians = Math.PI / 180.0 * ++index;
        var delay = TimeSpan.FromMilliseconds(idleDuration.TotalMilliseconds * Math.Sin(angleInRadians));
        delay += idleDuration;
        delay += minimumDelay;
        return delay;
    }

    Task Work(CancellationToken cancellationToken)
    {
        var sendOptions = new SendOptions();

        sendOptions.RouteToThisEndpoint();

        if (Random.Shared.Next(100) <= 10)
        {
            sendOptions.SetHeader("simulate-immediate-retry", bool.TrueString);
        }

        if (Random.Shared.Next(100) <= 5)
        {
            sendOptions.SetHeader("simulate-failure", bool.TrueString);
        }

        return messageSession.Send(new SomeMessage(), sendOptions, cancellationToken);
    }

    public void Stop() => cancellation.Cancel();


    CancellationTokenSource cancellation;
    int index;
}