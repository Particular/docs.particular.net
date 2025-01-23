namespace Sales;

public class SimulationEffects
{
    public void WriteState(TextWriter output)
    {
        output.WriteLine("Failure rate: {0:P0}", failureRate);
    }

    public void IncreaseFailureRate()
    {
        failureRate = Math.Min(1, failureRate + failureRateIncrement);
    }

    public void DecreaseFailureRate()
    {
        failureRate = Math.Max(0, failureRate - failureRateIncrement);
    }

    public Task SimulateMessageProcessing(CancellationToken cancellationToken = default)
    {
        if (Random.Shared.NextDouble() < failureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }

        return Task.Delay(baseProcessingTime, cancellationToken);
    }

    public void ProcessMessagesFaster()
    {
        if (baseProcessingTime > TimeSpan.Zero)
        {
            baseProcessingTime -= increment;
        }
    }

    public void ProcessMessagesSlower()
    {
        baseProcessingTime += increment;
    }

    TimeSpan baseProcessingTime = TimeSpan.FromMilliseconds(1300);
    TimeSpan increment = TimeSpan.FromMilliseconds(100);

    double failureRate;
    const double failureRateIncrement = 0.1;

    public void Reset()
    {
        failureRate = 0;
        baseProcessingTime = TimeSpan.Zero;
    }
}