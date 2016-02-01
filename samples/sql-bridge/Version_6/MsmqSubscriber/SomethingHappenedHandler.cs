using System;
using System.Threading.Tasks;
using Shared;
using NServiceBus;
#region msmqsubscriber-handler
public class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("MSMQ Subscriber has now received the event: SomethingHappened");
        return Task.FromResult(0);
    }
}
#endregion