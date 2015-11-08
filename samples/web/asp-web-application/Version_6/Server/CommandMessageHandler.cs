using NServiceBus;
using System;

#region Handler
public class CommandMessageHandler : IHandleMessages<Command>
{
    IBus bus;

    public CommandMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(Command message)
    {
        Console.WriteLine("Hello from CommandMessageHandler");

        if (message.Id%2 == 0)
        {
            bus.Return(ErrorCodes.Fail);
        }
        else
        {
            bus.Return(ErrorCodes.None);
        }
    }
}
#endregion