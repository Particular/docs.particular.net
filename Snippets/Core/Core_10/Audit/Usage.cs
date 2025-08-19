namespace Core.Audit;

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