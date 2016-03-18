namespace Snippets6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        void ConfigureFromCode(EndpointConfiguration endpointConfiguration)
        {
            #region TuningFromCode
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }
        
    }
}