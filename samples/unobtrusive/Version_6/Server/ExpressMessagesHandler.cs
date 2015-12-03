using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class ExpressMessagesHandler : IHandleMessages<RequestExpress>
{
    public Task Handle(RequestExpress message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message [{0}] received, id: [{1}]", message.GetType(), message.RequestId);
        return Task.FromResult(0);
    }

}