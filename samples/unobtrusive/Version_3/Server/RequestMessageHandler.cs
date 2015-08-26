using System;
using Messages;
using NServiceBus;

public class RequestMessageHandler : IHandleMessages<Request>
{
    IBus bus;

    public RequestMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Request message)
    {
        Console.WriteLine("Request received with id:" + message.RequestId);

        bus.Reply(new Response
        {
            ResponseId = message.RequestId
        });
    }
}