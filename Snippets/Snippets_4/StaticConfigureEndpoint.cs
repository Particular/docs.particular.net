using NServiceBus;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpointV4

        Configure.Endpoint.AsSendOnly();
        Configure.Endpoint.AsVolatile();
        Configure.Endpoint.Advanced(settings => settings.DisableDurableMessages());
        Configure.Endpoint.Advanced(settings => settings.EnableDurableMessages());

        #endregion
    }

}