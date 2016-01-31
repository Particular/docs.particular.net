using System;
using Shared;
using NServiceBus;
#region msmqsubscriber-handler
public class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public void Handle(SomethingHappened message)
    {
        Console.WriteLine("MSMQ Subscriber has now received the event: SomethingHappened");
    }
}
#endregion