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
        log.Error($"Received ServiceControl 'MessageFailed' event for a {message.MessageType} with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        log.Warn($"Heartbeats from {message.EndpointName} have stopped.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        log.Info($"Heartbeats from {message.EndpointName} have been restored.");
        return Task.CompletedTask;
    }
}

#endregion