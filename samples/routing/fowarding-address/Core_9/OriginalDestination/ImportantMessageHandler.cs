using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

#region old-handler

class ImportantMessageHandler :
    IHandleMessages<ImportantMessage>
{
    public Task Handle(ImportantMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Got a very important message");
        return Task.CompletedTask;
    }
}

#endregion