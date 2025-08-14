namespace Core.Transports;

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