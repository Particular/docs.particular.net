using System;
using NServiceBus;

#region handler
public class MyHandler : 
    IHandleMessages<MessageToSkipAudit>,
    IHandleMessages<MessageToAudit>
{
    public void Handle(MessageToAudit message)
    {
        Console.WriteLine("MessageToAudit received");
    }
    public void Handle(MessageToSkipAudit message)
    {
        Console.WriteLine("MessageToSkipAudit received");
    }
}
#endregion