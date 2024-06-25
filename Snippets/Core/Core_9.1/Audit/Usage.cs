namespace Core9.Audit
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