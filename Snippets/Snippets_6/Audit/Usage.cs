namespace Snippets6.Audit
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region AuditWithCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue");

            #endregion
        }


    }
}