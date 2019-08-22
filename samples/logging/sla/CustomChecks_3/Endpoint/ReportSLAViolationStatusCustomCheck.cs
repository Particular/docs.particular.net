using System;
using System.Threading.Tasks;
using NServiceBus.CustomChecks;

class ReportSLAViolationStatusCustomCheck : CustomCheck
{
    private readonly EstimatedTimeToSLABreachCounter counter;

    public ReportSLAViolationStatusCustomCheck(SLASettings settings, EstimatedTimeToSLABreachCounter counter) : base("SLA violation status", "Metrics", TimeSpan.FromSeconds(2))
    {
        this.settings = settings;
        this.counter = counter;
    }
    public override Task<CheckResult> PerformCheck()
    {
        var timeToBreachSLAInSeconds = counter.Recalculate();

        if (timeToBreachSLAInSeconds == 0)
        {
            return CheckResult.Failed($"SLA violation occurred on '{settings.EndpointName}'");
        }

        if (timeToBreachSLAInSeconds <= settings.TimeToNotifyAboutSLABreachToOccur.TotalSeconds)
        {
            return CheckResult.Failed($"SLA violation will occur in '{timeToBreachSLAInSeconds}' seconds on '{settings.EndpointName}'");
        }

        return CheckResult.Pass;
    }

    private readonly SLASettings settings;
}