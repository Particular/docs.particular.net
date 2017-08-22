using HandlerOrdering;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Usage

        endpointConfiguration.ApplyInterfaceHandlerOrdering();

        #endregion
    }

}