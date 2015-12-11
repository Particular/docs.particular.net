using System;
using System.Threading.Tasks;
using NServiceBus;

#region EnumMessageHandler
public class EnumMessageHandler : IHandleMessages<EnumMessage>
{
    public async Task Handle(EnumMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received, Returning");
        await context.Reply(Status.OK);
    }
}


#endregion