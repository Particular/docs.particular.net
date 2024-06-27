using System;
using NServiceBus;

class OverrideTTBR
{
    public OverrideTTBR(EndpointConfiguration endpointConfiguration)
    {
        #region audit-ttbr-override

        var auditTimeToBeReceived = TimeSpan.FromHours(4);
        endpointConfiguration.AuditProcessedMessagesTo("targetAuditQueue", timeToBeReceived: auditTimeToBeReceived);

        #endregion
    }
}