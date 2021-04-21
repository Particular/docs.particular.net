using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using NServiceBus;
using NServiceBus.Logging;
using ServiceControl.Contracts;

#region AzureMonitorConnectorEventsHandler

public class MessageFailedHandler :
    IHandleMessages<MessageFailed>
{
    readonly TelemetryClient telemetryClient;
    static ILog log = LogManager.GetLogger<MessageFailed>();

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