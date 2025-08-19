using NServiceBus.CustomChecks;

class SampleCustomCheck() : CustomCheck("Repeated Failure", "Sample", TimeSpan.FromSeconds(30))
{
    private int counter;

    public override Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
    {
        counter = (counter + 1) % 2;
        return counter == 1 ? CheckResult.Failed("Sometimes failures happen") : CheckResult.Pass;
    }
}