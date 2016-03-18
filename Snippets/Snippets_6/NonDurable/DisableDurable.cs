namespace Snippets6.NonDurable
{
    using NServiceBus;

    public class DisableDurable
    {
        public DisableDurable(EndpointConfiguration endpointConfiguration)
        {
            #region set-to-non-durable
            endpointConfiguration.DisableDurableMessages();

            #endregion
        }

    }
}