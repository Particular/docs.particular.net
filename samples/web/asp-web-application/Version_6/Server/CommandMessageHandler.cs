using NServiceBus;
using System;
using System.Threading.Tasks;

#region Handler
public class CommandMessageHandler : IHandleMessages<Command>
{
    public async Task Handle(Command message, IMessageHandlerContext context)
    {
        Console.WriteLine("Hello from CommandMessageHandler");

        if (message.Id % 2 == 0)
        {
            await context.Reply(ErrorCodes.Fail);
        }
        else
        {
            await context.Reply(ErrorCodes.None);
        }
    }
}
#endregion