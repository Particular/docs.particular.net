using NServiceBus;
using NServiceBus.AuditFilter;

#region MessageToIncludeAudit

[IncludeInAudit]
public class MessageToIncludeAudit :
    IMessage
{
}

#endregion