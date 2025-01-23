namespace Shipping;

public class SimulationEffects
{
    public void WriteState(TextWriter output)
    {
        output.Write("Simulated degrading resource: ");
        output.WriteLine(degradingResourceSimulationStarted.HasValue ? "ON" : "OFF");
        output.WriteLine("Failure rate: {0:P0}", failureRate);
    }

    public Task SimulateOrderBilledMessageProcessing(CancellationToken cancellationToken = default)
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

    public Task SimulateOrderPlacedMessageProcessing(CancellationToken cancellationToken = default)
    {
        if (Random.Shared.NextDouble() < failureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }

        var delay = TimeSpan.FromMilliseconds(200) + Degradation();
        return Task.Delay(delay, cancellationToken);
    }

    public void ToggleDegradationSimulation()
    {
        degradingResourceSimulationStarted = degradingResourceSimulationStarted.HasValue ? default(DateTime?) : DateTime.UtcNow;
    }

    TimeSpan Degradation()
    {
        var timeSinceDegradationStarted = DateTime.UtcNow - (degradingResourceSimulationStarted ?? DateTime.MaxValue);
        if (timeSinceDegradationStarted < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        return new TimeSpan(timeSinceDegradationStarted.Ticks / degradationRate);
    }

    public void IncreaseFailureRate()
    {
        failureRate = Math.Min(1, failureRate + failureRateIncrement);
    }

    public void DecreaseFailureRate()
    {
        failureRate = Math.Max(0, failureRate - failureRateIncrement);
    }

    TimeSpan baseProcessingTime = TimeSpan.FromMilliseconds(700);
    readonly TimeSpan increment = TimeSpan.FromMilliseconds(100);

    DateTime? degradingResourceSimulationStarted;
    const int degradationRate = 5;

    double failureRate;
    const double failureRateIncrement = 0.1;

    public void Reset()
    {
        failureRate = 0;
        baseProcessingTime = TimeSpan.Zero;
    }
}