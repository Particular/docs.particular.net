using System;
using NServiceBus;
using V1.Messages;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    public void Handle(ISomethingHappened message)
    {
        Console.WriteLine("Something happened with some data {0} and no more info", message.SomeData);
    }
}