namespace Snippets6.Audit
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region AuditWithCode

            endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue");

            #endregion
        }


    }
}