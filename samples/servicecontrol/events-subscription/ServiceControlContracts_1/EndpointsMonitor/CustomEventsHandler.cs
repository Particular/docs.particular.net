using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

#region ServiceControlEventsHandlers

public class CustomEventsHandler :
    IHandleMessages<MessageFailed>,
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>

{
    static ILog log = LogManager.GetLogger<CustomEventsHandler>();

    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {
        log.Error("Received ServiceControl 'MessageFailed' event for a SimpleMessage.");

        return Task.FromResult(0);
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        log.Warn($"Heartbeat from {message.EndpointName} stopped.");

        return Task.FromResult(0);
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        log.Info($"Heartbeat from {message.EndpointName} restored.");

        return Task.FromResult(0);
    }
}

#endregion