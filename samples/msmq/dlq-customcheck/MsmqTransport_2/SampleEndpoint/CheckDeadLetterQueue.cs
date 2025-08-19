using NServiceBus.CustomChecks;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#region check-dead-letter-queue

sealed class CheckDeadLetterQueue :
    CustomCheck
{
    PerformanceCounter dlqPerformanceCounter;

    public CheckDeadLetterQueue() :
        base("Dead Letter Queue", "Transport", TimeSpan.FromMinutes(1))
    {
        dlqPerformanceCounter = new PerformanceCounter(
            categoryName: "MSMQ Queue",
            counterName: "Messages in Queue",
            instanceName: "Computer Queues",
            readOnly: true
        );
    }

    public override Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
    {
        var currentValue = dlqPerformanceCounter.NextValue();

        return currentValue <= 0
            ? CheckResult.Pass
            : CheckResult.Failed($"{currentValue} messages in the Dead Letter Queue on {Environment.MachineName}");
    }
}

#endregion