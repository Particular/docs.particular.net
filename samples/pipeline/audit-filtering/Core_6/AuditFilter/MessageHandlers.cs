using System;
using System.Threading.Tasks;
using NServiceBus;

public class AuditThisMessageHandler : IHandleMessages<AuditThisMessage>
{
    public async Task Handle(AuditThisMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling {message.GetType().Name}");
    }
}

public class DoNotAuditThisMessageHandler : IHandleMessages<DoNotAuditThisMessage>
{
    public async Task Handle(DoNotAuditThisMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Handling {message.GetType().Name}");
    }
}