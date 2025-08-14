namespace Core.Outbox;

using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxEnablineInCode

        endpointConfiguration.EnableOutbox();

        #endregion
    }

}