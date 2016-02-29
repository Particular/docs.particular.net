namespace Snippets6.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region OutboxEnablineInCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.EnableOutbox();

            #endregion
        }

    }
}