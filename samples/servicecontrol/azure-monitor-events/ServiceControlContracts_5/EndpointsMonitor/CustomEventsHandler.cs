using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using NServiceBus;
using ServiceControl.Contracts;

#region AzureMonitorConnectorEventsHandler

public class MessageFailedHandler :
    IHandleMessages<MessageFailed>
{
    readonly TelemetryClient telemetryClient;
    private readonly ILogger<MessageFailedHandler> logger;

    public MessageFailedHandler(TelemetryClient telemetryClient, ILogger<MessageFailedHandler> logger)
    {
        this.telemetryClient = telemetryClient;
        this.logger = logger;
    }

    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {

        telemetryClient.TrackEvent("Message Failed", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        logger.LogError("Received ServiceControl 'MessageFailed' event for a {MessageType} with ID {FailedMessageId}.", message.MessageType, message.FailedMessageId);
        return Task.CompletedTask;
    }
}

#endregion

public class CustomEventsHandler :
    IHandleMessages<HeartbeatStopped>,
    IHandleMessages<HeartbeatRestored>,
    IHandleMessages<FailedMessagesArchived>,
    IHandleMessages<FailedMessagesUnArchived>,
    IHandleMessages<MessageFailureResolvedByRetry>,
    IHandleMessages<MessageFailureResolvedManually>
{
    readonly TelemetryClient telemetryClient;
    private readonly ILogger<CustomEventsHandler> logger;

    public CustomEventsHandler(TelemetryClient telemetryClient, ILogger<CustomEventsHandler> logger)
    {
        this.telemetryClient = telemetryClient;
        this.logger = logger;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Heartbeat Stopped", new Dictionary<string, string>
        {
            {"EndpointName", message.EndpointName},
        });

        logger.LogWarning("Heartbeats from {EndpointName} have stopped.", message.EndpointName);
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Heartbeat Restored", new Dictionary<string, string>
        {
            {"EndpointName", message.EndpointName},
        });

        logger.LogInformation("Heartbeats from {EndpointName} have been restored.", message.EndpointName);
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesArchived message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Failed Messages Archived", new Dictionary<string, string>
        {
            {"MessagesIds", string.Join(",", message.FailedMessagesIds)},
        });

        logger.LogError("Received ServiceControl 'FailedMessageArchived' with ID {FailedMessageId}.", message.FailedMessagesIds.FirstOrDefault());
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesUnArchived message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Failed Messages Unarchived", new Dictionary<string, string>
        {
            {"MessagesIds", string.Join(",", message.FailedMessagesIds)},
        });

        logger.LogError("Received ServiceControl 'FailedMessagesUnArchived' MessagesCount: {Count}.", message.FailedMessagesIds.Length);
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedByRetry message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Message Failure Resolved By Retry", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        logger.LogError("Received ServiceControl 'MessageFailureResolvedByRetry' with ID {FailedMessageId}.", message.FailedMessageId);
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedManually message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Message Failure Resolved Manually", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        logger.LogError("Received ServiceControl 'MessageFailureResolvedManually'  with ID {FailedMessageId}.", message.FailedMessageId);
        return Task.CompletedTask;
    }
}