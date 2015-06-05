using System;
using NServiceBus;

#region CommandMessageHandler
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
            Console.WriteLine("Returning Fail");
            bus.Return(ErrorCodes.Fail);
        }
        else
        {
            Console.WriteLine("Returning None");
            bus.Return(ErrorCodes.None);
        }
    }
}
#endregion
