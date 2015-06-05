using System;
using Shared;
using NServiceBus;
#region sqlsubscriber-handler
class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
{
    public void Handle(SomethingHappened message)
    {
        Console.WriteLine("Sql subscriber has now received this event. This was originally published by MSMQ publisher.");
    }
}
#endregion