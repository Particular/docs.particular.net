using System;
using System.Diagnostics;
using ServiceControl.Plugin.CustomChecks;

#region check-dead-letter-queue

class CheckDeadLetterQueue :
    PeriodicCheck
{
    PerformanceCounter dlqPerformanceCounter;

    public CheckDeadLetterQueue() :
        base("Dead Letter Queue", "Transport", TimeSpan.FromMinutes(1))
    {
        dlqPerformanceCounter = new PerformanceCounter(
            categoryName: "MSMQ Queue",
            counterName: "Messages in Queue",
            instanceName: "Computer Queues",
            readOnly: true);
    }

    public override CheckResult PerformCheck()
    {
        var currentValue = dlqPerformanceCounter.NextValue();

        if (currentValue <= 0)
        {
            return CheckResult.Pass;
        }
        return CheckResult.Failed($"{currentValue} messages in the Dead Letter Queue on {Environment.MachineName}");
    }
}

#endregion