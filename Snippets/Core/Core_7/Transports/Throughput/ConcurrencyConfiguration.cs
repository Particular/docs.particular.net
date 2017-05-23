namespace Core6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        ConcurrencyConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region TuningFromCode
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(5);
            #endregion

            #region TuningTimeoutManagerConcurrency 6.1

            var timeoutManager = endpointConfiguration.TimeoutManager();
            timeoutManager.LimitMessageProcessingConcurrencyTo(4);
            #endregion
        }

    }
}