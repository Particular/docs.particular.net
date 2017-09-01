using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ServiceControl.Plugin.CustomChecks;

#region check-dead-letter-queue

class CheckDeadLetterQueue : CustomCheck
{
    PerformanceCounter dlqPerformanceCounter;

    public CheckDeadLetterQueue() : base("Dead Letter Queue", "Transport", TimeSpan.FromMinutes(1))
    {
        dlqPerformanceCounter = new PerformanceCounter(
            "MSMQ Queue",
            "Messages in Queue",
            "Computer Queues",
            true);
    }

    public override Task<CheckResult> PerformCheck()
    {
        var currentValue = dlqPerformanceCounter.NextValue();

        if (currentValue > 0)
        {
            return CheckResult.Failed($"{currentValue} messages in the Dead Letter Queue on {Environment.MachineName}");
        }

        return CheckResult.Pass;
    }
}

#endregion