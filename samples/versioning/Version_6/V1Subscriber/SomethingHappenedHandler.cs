using System;
using System.Threading.Tasks;
using NServiceBus;
using V1.Messages;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("Something happened with some data {0} and no more info", message.SomeData);
        return Task.FromResult(0);
    }
}