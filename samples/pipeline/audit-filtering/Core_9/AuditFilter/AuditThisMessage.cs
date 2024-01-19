using NServiceBus;

public class AuditThisMessage :
    IMessage
{
    public string Content { get; set; }
}