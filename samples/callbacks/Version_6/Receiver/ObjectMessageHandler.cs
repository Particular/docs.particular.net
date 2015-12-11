using System;
using System.Threading.Tasks;
using NServiceBus;

#region ObjectMessageHandler

public class ObjectMessageHandler : IHandleMessages<ObjectMessage>
{
    public async Task Handle(ObjectMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received, Returning");
        await context.Reply(new ObjectResponseMessage
        {
            Property = "PropertyValue"
        });
    }
}

#endregion