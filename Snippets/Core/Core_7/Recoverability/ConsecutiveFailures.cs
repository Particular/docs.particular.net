namespace Core7.Recoverability
{
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
                    onRateLimitStarted: () => Console.Out.WriteLineAsync("Rate limiting started"),
                    onRateLimitEnded: () => Console.Out.WriteLineAsync("Rate limiting stopped")));

            #endregion
        }
    }
}
