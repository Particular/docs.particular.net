namespace Snippets6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        void ConfigureFromCode()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region TuningFromCode
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }
        
    }
}