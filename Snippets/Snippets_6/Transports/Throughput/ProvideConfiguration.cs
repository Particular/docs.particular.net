namespace Snippets6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        void ConfigureFromCode()
        {
            #region TuningFromCode
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }
        
    }
}