namespace Snippets4
{
    using NServiceBus;

    public class StaticConfigureEndpoint
    {
        public void Simple()
        {
            #region StaticConfigureEndpoint

            Configure.Endpoint.AsSendOnly();
            Configure.Endpoint.AsVolatile();
            Configure.Endpoint.Advanced(settings => settings.DisableDurableMessages());
            Configure.Endpoint.Advanced(settings => settings.EnableDurableMessages());

            #endregion
        }

    }
}