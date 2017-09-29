using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

#region new-handler

class ImportantMessageHandler :
    IHandleMessages<ImportantMessage>
{
    public Task Handle(ImportantMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Got a very important message from the new handler!");
        return Task.CompletedTask;
    }
}

#endregion