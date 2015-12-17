using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class RequestMessageHandler : IHandleMessages<Request>
{
    public Task Handle(Request message, IMessageHandlerContext context)
    {
        Console.WriteLine("Request received with id:" + message.RequestId);

        context.Reply(new Response
                        {
                            ResponseId = message.RequestId
                        });
        return Task.FromResult(0);
    }
}
