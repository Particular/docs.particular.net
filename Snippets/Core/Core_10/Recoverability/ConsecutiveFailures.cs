namespace Core.Recoverability;

using System;
using NServiceBus;

public class ConfigureConsecutiveFailures
{
    public static void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region configure-consecutive-failures

        endpointConfiguration.Recoverability().OnConsecutiveFailures(10,
            new RateLimitSettings(
                timeToWaitBetweenThrottledAttempts: TimeSpan.FromSeconds(5),
                onRateLimitStarted: ct => Console.Out.WriteLineAsync("Rate limiting started"),
                onRateLimitEnded: cts => Console.Out.WriteLineAsync("Rate limiting stopped")));

        #endregion
    }
}