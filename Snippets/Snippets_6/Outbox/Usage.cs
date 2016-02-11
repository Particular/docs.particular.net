namespace Snippets6.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region OutboxEnablineInCode

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.EnableOutbox();

            #endregion
        }

    }
}