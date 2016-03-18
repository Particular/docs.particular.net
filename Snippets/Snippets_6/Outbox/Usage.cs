namespace Snippets6.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxEnablineInCode

            endpointConfiguration.EnableOutbox();

            #endregion
        }

    }
}