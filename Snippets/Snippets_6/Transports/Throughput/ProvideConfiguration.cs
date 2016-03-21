namespace Snippets6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        ConcurrencyConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region TuningFromCode
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }
        
    }
}