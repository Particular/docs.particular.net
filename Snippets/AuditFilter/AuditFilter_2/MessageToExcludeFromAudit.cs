using NServiceBus;
using NServiceBus.AuditFilter;

#region MessageToExcludeFromAudit

[ExcludeFromAudit]
public class MessageToExcludeFromAudit :
    IMessage
{
}

#endregion