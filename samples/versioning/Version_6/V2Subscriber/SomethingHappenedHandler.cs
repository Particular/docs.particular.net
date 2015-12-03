using System;
using System.Threading.Tasks;
using V2.Messages;
using NServiceBus;

public class SomethingHappenedHandler : IHandleMessages<ISomethingHappened>
{
    public Task Handle(ISomethingHappened message, IMessageHandlerContext context)
    {
        Console.WriteLine("Something happened with some data {0} and more information {1}", message.SomeData, message.MoreInfo);
        return Task.FromResult(0);
    }

}