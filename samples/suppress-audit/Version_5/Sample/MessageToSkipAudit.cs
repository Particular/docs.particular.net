using NServiceBus;

#region message-to-skip-audit

[SkipAudit]
public class MessageToSkipAudit : ICommand
{
}

#endregion

