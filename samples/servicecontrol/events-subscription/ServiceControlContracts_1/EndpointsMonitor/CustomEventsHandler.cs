using System;
using System.Threading.Tasks;
using NServiceBus;
using ServiceControl.Contracts;

#region ServiceControlEventsHandlers

public class CustomEventsHandler :
    IHandleMessages<MessageFailed>,
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>

{
    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Received ServiceControl 'MessageFailed' event for a SimpleMessage.");

        return Task.FromResult(0);
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Heartbeat from {message.EndpointName} stopped.");

        return Task.FromResult(0);
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Heartbeat from {message.EndpointName} restored.");

        return Task.FromResult(0);
    }
}

#endregion