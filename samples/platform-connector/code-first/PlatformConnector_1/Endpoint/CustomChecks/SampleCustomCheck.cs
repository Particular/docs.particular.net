using System;
using System.Threading.Tasks;
using NServiceBus.CustomChecks;

class SampleCustomCheck : CustomCheck
{
    int counter;

    public SampleCustomCheck()
        : base("Repeated Failure", "Sample", TimeSpan.FromSeconds(30))
    {
    }

    public override Task<CheckResult> PerformCheck()
    {
        counter = (counter + 1) % 2;
        if(counter == 1)
        {
            return CheckResult.Failed("Sometimes failures happen");
        }

        return CheckResult.Pass;
    }
}