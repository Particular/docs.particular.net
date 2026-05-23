namespace Core;

using NServiceBus;

class SendOnly
{
    void Configure()
    {
        #region send-only-endpoint

        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        endpointConfiguration.SendOnly();

        #endregion
    }
}
