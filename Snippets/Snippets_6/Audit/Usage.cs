namespace Snippets6.Audit
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region AuditWithCode

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.AuditProcessedMessagesTo("targetAuditQueue");

            #endregion
        }


    }
}