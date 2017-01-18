using System;
using System.Threading.Tasks;
using NServiceBus;

class ReplyHandler :
    IHandleMessages<ReplyMessage>
{
    public Task Handle(ReplyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Got reply from {message.SomeId}");
        return context.Reply(new ReplyFollowUpMessage());
    }
}