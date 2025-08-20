using Microsoft.Extensions.Logging;
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
        logger.LogError("Received ServiceControl 'MessageFailed' event for a {MessageType} with ID {FailedMessageId}.", message.MessageType, message.FailedMessageId);
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        logger.LogWarning("Heartbeats from {EndpointName} have stopped.", message.EndpointName);
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        logger.LogInformation("Heartbeats from {EndpointName} have been restored.", message.EndpointName);
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesArchived message, IMessageHandlerContext context)
    {
        logger.LogError("Received ServiceControl 'FailedMessageArchived' with ID {FailedMessageId}.", message.FailedMessagesIds.FirstOrDefault());
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesUnArchived message, IMessageHandlerContext context)
    {
        logger.LogError("Received ServiceControl 'FailedMessagesUnArchived' MessagesCount: {Count}.", message.FailedMessagesIds.Length);
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedByRetry message, IMessageHandlerContext context)
    {
        logger.LogError("Received ServiceControl 'MessageFailureResolvedByRetry' with ID {FailedMessageId}.", message.FailedMessageId);
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedManually message, IMessageHandlerContext context)
    {
        logger.LogError("Received ServiceControl 'MessageFailureResolvedManually'  with ID {FailedMessageId}.", message.FailedMessageId);
        return Task.CompletedTask;
    }
}

#endregion