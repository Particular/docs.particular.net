using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

#region ServiceControlEventsHandlers

public class CustomEventsHandler :
    IHandleMessages<MessageFailed>,
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>,
    IHandleMessages<FailedMessagesArchived>,
    IHandleMessages<FailedMessagesUnArchived>,
    IHandleMessages<MessageFailureResolvedByRetry>,
    IHandleMessages<MessageFailureResolvedManually>
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

    public Task Handle(FailedMessagesArchived message, IMessageHandlerContext context)
    {
        log.Error($"Received ServiceControl 'FailedMessageArchived' with ID {message.FailedMessagesIds.FirstOrDefault()}.");
        return Task.CompletedTask;
    }
    
    public Task Handle(FailedMessagesUnArchived message, IMessageHandlerContext context)
    {
        log.Error($"Received ServiceControl 'FailedMessagesUnArchived' MessagesCount: {message.FailedMessagesIds.Length}.");
        return Task.CompletedTask;
    }   
    
    public Task Handle(MessageFailureResolvedByRetry message, IMessageHandlerContext context)
    {
        log.Error($"Received ServiceControl 'MessageFailureResolvedByRetry' with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedManually message, IMessageHandlerContext context)
    {
        log.Error($"Received ServiceControl 'MessageFailureResolvedManually'  with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }
}

#endregion