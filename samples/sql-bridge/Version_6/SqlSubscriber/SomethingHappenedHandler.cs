using System;
using System.Threading.Tasks;
using Shared;
using NServiceBus;
#region sqlsubscriber-handler
class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public Task Handle(SomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("Sql subscriber has now received this event. This was originally published by MSMQ publisher.");
        return Task.FromResult(0);
    }
}
#endregion