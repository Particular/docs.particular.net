using System;
using System.Threading.Tasks;
using NServiceBus;

public class Handler : 
    IHandleMessages<EnumMessage>,
    IHandleMessages<IntMessage>,
    IHandleMessages<ObjectMessage>
{

    public async Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        string format = $"Received EnumMessage. Property:'{message.Property}'";
        Console.WriteLine(format);
        await context.Reply(Status.Ok);
    }

    public async Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        string format = $"Received IntMessage. Property:'{message.Property}'";
        Console.WriteLine(format);
        await context.Reply(10);
    }

    public async Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        string format = $"Received ObjectMessage. Property:'{message.Property}'";
        Console.WriteLine(format);
        await context.Reply(new ReplyMessage
        {
            Property = $"Handler Received '{message.Property}'"
        });
    }
}