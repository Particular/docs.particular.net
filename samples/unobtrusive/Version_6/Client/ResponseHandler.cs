using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class ResponseHandler : IHandleMessages<Response>
{
    public Task Handle(Response message, IMessageHandlerContext context)
    {
        Console.WriteLine("Response received from server for request with id:" + message.ResponseId);
        return Task.FromResult(0);
    }

}