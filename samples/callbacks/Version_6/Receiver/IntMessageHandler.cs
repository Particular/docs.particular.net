using System;
using System.Threading.Tasks;
using NServiceBus;

#region IntMessageHandler
public class IntMessageHandler : IHandleMessages<IntMessage>
{
    public async Task Handle(IntMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received, Returning");
        await context.Reply(10);
    }
}


#endregion