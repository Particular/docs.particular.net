using System;
using V2.Messages;
using NServiceBus;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    public void Handle(ISomethingHappened message)
    {
        Console.WriteLine("Something happened with some data {0} and more information {1}", message.SomeData, message.MoreInfo);
    }
}