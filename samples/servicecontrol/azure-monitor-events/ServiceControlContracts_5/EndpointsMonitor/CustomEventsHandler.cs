using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

#region AzureMonitorConnectorEventsHandler

public class MessageFailedHandler :
    IHandleMessages<MessageFailed>
{
    readonly TelemetryClient telemetryClient;
    static ILog log = LogManager.GetLogger<CustomEventsHandler>();

    public MessageFailedHandler(TelemetryClient telemetryClient)
    {
        this.telemetryClient = telemetryClient;
    }

    public Task Handle(MessageFailed message, IMessageHandlerContext context)
    {
        
        telemetryClient.TrackEvent("Message Failed", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        log.Error($"Received ServiceControl 'MessageFailed' event for a {message.MessageType} with ID {message.FailedMessageId}.");
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
    static ILog log = LogManager.GetLogger<CustomEventsHandler>();

    public CustomEventsHandler(TelemetryClient telemetryClient)
    {
        this.telemetryClient = telemetryClient;
    }

    public Task Handle(HeartbeatStopped message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Heartbeat Stopped", new Dictionary<string, string>
        {
            {"EndpointName", message.EndpointName},
        });

        log.Warn($"Heartbeats from {message.EndpointName} have stopped.");
        return Task.CompletedTask;
    }

    public Task Handle(HeartbeatRestored message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Heartbeat Restored", new Dictionary<string, string>
        {
            {"EndpointName", message.EndpointName},
        });

        log.Info($"Heartbeats from {message.EndpointName} have been restored.");
        return Task.CompletedTask;
    }

    public Task Handle(FailedMessagesArchived message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Failed Messages Archived", new Dictionary<string, string>
        {
            {"MessagesIds", string.Join(",", message.FailedMessagesIds)},
        });

        log.Error($"Received ServiceControl 'FailedMessageArchived' with ID {message.FailedMessagesIds.FirstOrDefault()}.");
        return Task.CompletedTask;
    }
    
    public Task Handle(FailedMessagesUnArchived message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Failed Messages Unarchived", new Dictionary<string, string>
        {
            {"MessagesIds", string.Join(",", message.FailedMessagesIds)},
        });

        log.Error($"Received ServiceControl 'FailedMessagesUnArchived' MessagesCount: {message.FailedMessagesIds.Length}.");
        return Task.CompletedTask;
    }   
    
    public Task Handle(MessageFailureResolvedByRetry message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Message Failure Resolved By Retry", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        log.Error($"Received ServiceControl 'MessageFailureResolvedByRetry' with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }

    public Task Handle(MessageFailureResolvedManually message, IMessageHandlerContext context)
    {
        telemetryClient.TrackEvent("Message Failure Resolved Manually", new Dictionary<string, string>
        {
            {"MessageId", message.FailedMessageId},
        });

        log.Error($"Received ServiceControl 'MessageFailureResolvedManually'  with ID {message.FailedMessageId}.");
        return Task.CompletedTask;
    }
}