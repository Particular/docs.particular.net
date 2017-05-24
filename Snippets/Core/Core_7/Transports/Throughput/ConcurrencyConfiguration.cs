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

            #region TuningTimeoutManagerConcurrency

            var timeoutManager = endpointConfiguration.TimeoutManager();
            timeoutManager.LimitMessageProcessingConcurrencyTo(4);
            #endregion
        }

    }
}