using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using ServiceControl.Contracts;

#region ServiceControlEventsHandlers

public class CustomEventsHandler(ILogger<CustomEventsHandler> logger) :
    IHandleMessages<MessageFailed>,
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>,
    IHandleMessages<FailedMessagesArchived>,
    IHandleMessages<FailedMessagesUnArchived>,
    IHandleMessages<MessageFailureResolvedByRetry>,
    IHandleMessages<MessageFailureResolvedManually>
{
   
    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {
        logger.LogError($"Received ServiceControl 'MessageFailed' event for a {message.MessageType} with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        logger.LogWarning($"Heartbeats from {message.EndpointName} have stopped.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Heartbeats from {message.EndpointName} have been restored.");
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesArchived message, IMessageHandlerContext context)
    {
        logger.LogError($"Received ServiceControl 'FailedMessageArchived' with ID {message.FailedMessagesIds.FirstOrDefault()}.");
        return Task.CompletedTask;
    }
    
    public Task Handle(FailedMessagesUnArchived message, IMessageHandlerContext context)
    {
        logger.LogError($"Received ServiceControl 'FailedMessagesUnArchived' MessagesCount: {message.FailedMessagesIds.Length}.");
        return Task.CompletedTask;
    }   
    
    public Task Handle(MessageFailureResolvedByRetry message, IMessageHandlerContext context)
    {
        logger.LogError($"Received ServiceControl 'MessageFailureResolvedByRetry' with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedManually message, IMessageHandlerContext context)
    {
        logger.LogError($"Received ServiceControl 'MessageFailureResolvedManually'  with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }
}

#endregion