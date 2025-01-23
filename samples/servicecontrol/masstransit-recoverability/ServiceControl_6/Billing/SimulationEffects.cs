namespace Billing;

public class SimulationEffects
{
    public void IncreaseFailureRate() => failureRate = Math.Min(1, failureRate + FailureRateIncrement);

    public void DecreaseFailureRate() => failureRate = Math.Max(0, failureRate - FailureRateIncrement);

    public void WriteState(TextWriter output) => output.WriteLine("Failure rate: {0:P0}", failureRate);

    public async Task SimulatedMessageProcessing(CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken);

        if (Random.Shared.NextDouble() < failureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }
    }

    double failureRate = 0.5;
    const double FailureRateIncrement = 0.1;

    public void Reset()
    {
        failureRate = 0;
    }
}