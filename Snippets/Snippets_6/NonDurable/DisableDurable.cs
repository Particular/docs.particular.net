namespace Snippets6.NonDurable
{
    using NServiceBus;

    public class DisableDurable
    {
        public DisableDurable()
        {
            #region set-to-non-durable

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.DisableDurableMessages();

            #endregion
        }

    }
}