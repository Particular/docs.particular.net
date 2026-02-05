namespace Core.Audit;

using System;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AuditWithCode

        endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue");

        #endregion

        #region OverrideTimeToBeReceived

        endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue", TimeSpan.FromMinutes(30));

        #endregion
    }
}
