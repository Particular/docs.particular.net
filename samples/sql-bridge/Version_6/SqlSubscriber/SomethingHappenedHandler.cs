#region sqlsubscriber-handler

using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public async Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("Sql Subscriber has now received this event from the SqlRelay.");
    }
}

#endregion