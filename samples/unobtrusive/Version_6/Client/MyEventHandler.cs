using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;

public class MyEventHandler : IHandleMessages<IMyEvent>
{
    public Task Handle(IMyEvent message, IMessageHandlerContext context)
    {
        Console.WriteLine("IMyEvent received from server with id:" + message.EventId);
        return Task.FromResult(0);
    }

}