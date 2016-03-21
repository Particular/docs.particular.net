namespace Snippets6.Audit
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AuditWithCode

            endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue");

            #endregion
        }


    }
}