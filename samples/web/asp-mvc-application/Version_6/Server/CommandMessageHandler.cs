using System;
using System.Threading.Tasks;
using NServiceBus;

#region CommandMessageHandler
public class CommandMessageHandler : IHandleMessages<Command>
{
    public async Task Handle(Command message, IMessageHandlerContext context)
    {
        Console.WriteLine("Hello from CommandMessageHandler");

        if (message.Id % 2 == 0)
        {
            Console.WriteLine("Returning Fail");
            await context.Reply(ErrorCodes.Fail);
        }
        else
        {
            Console.WriteLine("Returning None");
            await context.Reply(ErrorCodes.None);
        }
    }
}
#endregion
